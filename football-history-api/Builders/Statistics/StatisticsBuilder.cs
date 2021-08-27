using System;
using System.Collections.Generic;
using System.Linq;
using football.history.api.Builders.Team;
using football.history.api.Repositories.Competition;
using football.history.api.Repositories.Match;
using MoreLinq;

namespace football.history.api.Builders.Statistics
{
    public interface IStatisticsBuilder
    {
        List<StatisticDto> BuildSeasonStatistics(long seasonId);
    }

    public class StatisticsBuilder : IStatisticsBuilder
    {
        private readonly IMatchRepository _matchRepository;
        private readonly ICompetitionRepository _competitionRepository;

        public StatisticsBuilder(IMatchRepository matchRepository, ICompetitionRepository competitionRepository)
        {
            _matchRepository = matchRepository;
            _competitionRepository = competitionRepository;
        }

        public List<StatisticDto> BuildSeasonStatistics(long seasonId)
        {
            var matches = _matchRepository.GetMatches(
                competitionId: null,
                seasonId,
                teamId: null,
                MatchType.League,
                matchDate: null);

            var competitions = _competitionRepository.GetCompetitionsInSeason(seasonId);
            var pointsForWin = competitions.Select(x => x.PointsForWin).Distinct().ToList();
            if (pointsForWin.Count != 1)
            {
                throw new InvalidOperationException(
                    $"Expected points for win to be the same for all competitions in a single season ({seasonId})");
            }

            var statistics = new List<StatisticDto>();

            var metrics = CalculateResultMetrics(matches);

            statistics.Add(GetMostPointsStatistic(metrics, pointsForWin.Single()));
            statistics.Add(GetFewestPointsStatistic(metrics, pointsForWin.Single()));
            statistics.Add(GetBestPointsPerGameStatistic(metrics, pointsForWin.Single()));

            statistics.Add(GetMostWinsStatistic(metrics));
            statistics.Add(GetMostDrawsStatistic(metrics));
            statistics.Add(GetMostLossesStatistic(metrics));

            statistics.Add(GetMostGoalsStatistic(metrics));
            statistics.Add(GetFewestGoalsStatistic(metrics));
            statistics.Add(GetBestGoalDifferenceStatistic(metrics));
            statistics.Add(GetBestGoalAverageStatistic(metrics));

            //TODO: This definitely isn't right at the mo...
            var consecutiveMetrics = GetConsecutiveMetrics(matches);
            
            statistics.Add(GetMostConsecutiveWins(consecutiveMetrics));
            statistics.Add(GetMostConsecutiveDraws(consecutiveMetrics));
            statistics.Add(GetMostConsecutiveLosses(consecutiveMetrics));
            statistics.Add(GetMostGamesWithoutDefeat(consecutiveMetrics));
            statistics.Add(GetMostGamesWithoutWin(consecutiveMetrics));

            return statistics;
        }

        private StatisticDto GetMostConsecutiveWins(List<ConsecutiveResult> metrics)
        {
            var mostConsecutiveWins = metrics.MaxBy(x => x.ConsecutiveWins);
            return new StatisticDto(
                "Results",
                "Most Consecutive Wins",
                mostConsecutiveWins.First().ConsecutiveWins,
                GetTeamNames(mostConsecutiveWins),
                GetCompetitionNames(mostConsecutiveWins));
        }

        private StatisticDto GetMostConsecutiveDraws(List<ConsecutiveResult> metrics)
        {
            var mostConsecutiveDraws = metrics.MaxBy(x => x.ConsecutiveDraws);
            return new StatisticDto(
                "Results",
                "Most Consecutive Draws",
                mostConsecutiveDraws.First().ConsecutiveDraws,
                GetTeamNames(mostConsecutiveDraws),
                GetCompetitionNames(mostConsecutiveDraws));
        }

        private StatisticDto GetMostConsecutiveLosses(List<ConsecutiveResult> metrics)
        {
            var mostConsecutiveLosses = metrics.MaxBy(x => x.ConsecutiveLosses);
            return new StatisticDto(
                "Results",
                "Most Consecutive Losses",
                mostConsecutiveLosses.First().ConsecutiveLosses,
                GetTeamNames(mostConsecutiveLosses),
                GetCompetitionNames(mostConsecutiveLosses));
        }

        private StatisticDto GetMostGamesWithoutDefeat(List<ConsecutiveResult> metrics)
        {
            var winningStreak = metrics.MaxBy(x => x.WinningStreak);
            return new StatisticDto(
                "Results",
                "Most Games Without Defeat",
                winningStreak.First().WinningStreak,
                GetTeamNames(winningStreak),
                GetCompetitionNames(winningStreak));
        }
        
        private StatisticDto GetMostGamesWithoutWin(List<ConsecutiveResult> metrics)
        {
            var losingStreak = metrics.MaxBy(x => x.LosingStreak);
            return new StatisticDto(
                "Results",
                "Most Games Without Win",
                losingStreak.First().LosingStreak,
                GetTeamNames(losingStreak),
                GetCompetitionNames(losingStreak));
        }

        private List<ConsecutiveResult> GetConsecutiveMetrics(List<MatchModel> matches)
        {
            var homeGroup = matches
                .GroupBy(match => (teamId: match.HomeTeamId, teamName: match.HomeTeamName,
                    competitionName: match.CompetitionName)).Select(group => new
                {
                    Info = group.Key,
                    ResultSequence = group.Select(x => new
                    {
                        Date = x.MatchDate,
                        Result = x.HomeGoals > x.AwayGoals ? "W" : x.HomeGoals < x.AwayGoals ? "L" : "D"
                    })
                });

            var awayGroup = matches
                .GroupBy(match => (teamId: match.AwayTeamId, teamName: match.AwayTeamName,
                    competitionName: match.CompetitionName)).Select(group => new
                {
                    Info = group.Key,
                    ResultSequence = group.Select(x => new
                    {
                        Date = x.MatchDate,
                        Result = x.AwayGoals > x.HomeGoals ? "W" : x.AwayGoals < x.HomeGoals ? "L" : "D"
                    })
                });

            var combined = homeGroup
                .Join(awayGroup,
                    home => home.Info.teamId,
                    away => away.Info.teamId,
                    (home, away) => new
                    {
                        Info = home.Info,
                        ResultSequence = home.ResultSequence.Concat(away.ResultSequence).OrderBy(x => x.Date)
                    })
                .ToList();

            var calcResult = new List<ConsecutiveResult>();
            foreach (var item in combined)
            {
                var teamName = item.Info.teamName;
                var numWins = 0;
                var numDraws = 0;
                var numLosses = 0;
                var nonLosingStreak = 0;
                var nonWinningStreak = 0;

                var currentRun = "";
                var count = 0;
                var streakCount = 0;
                var streakType = ""; // Can be W or L
                foreach (var game in item.ResultSequence)
                {
                    if (game.Result == currentRun)
                    {
                        count++;
                        streakCount++;
                    }
                    else if (game.Result == "D")
                    {
                        streakCount++;

                        if (currentRun == "W")
                        {
                            numWins = count > numWins ? count : numWins;
                        }

                        if (currentRun == "L")
                        {
                            numLosses = count > numLosses ? count : numLosses;
                        }

                        count = 1;
                        currentRun = game.Result;
                    }
                    else
                    {
                        if (currentRun == "W")
                        {
                            numWins = count > numWins ? count : numWins;
                            nonLosingStreak = streakCount > nonLosingStreak ? streakCount : nonLosingStreak;
                            streakType = game.Result;
                            streakCount = 1;
                        }

                        if (currentRun == "L")
                        {
                            numLosses = count > numLosses ? count : numLosses;
                            nonWinningStreak = streakCount > nonWinningStreak ? streakCount : nonWinningStreak;
                            streakType = game.Result;
                            streakCount = 1;
                        }

                        if (currentRun == "D")
                        {
                            numDraws = count > numDraws ? count : numDraws;

                            if (streakType == game.Result || streakType == "")
                            {
                                streakCount++;
                                streakType = game.Result;
                            }
                            else
                            {
                                streakType = game.Result;
                                streakCount = 1 + count; // ensure a sequence of L D W results in a non-losing streak of 2 
                            }
                        }

                        if (currentRun == "")
                        {
                            streakCount++;
                            streakType = game.Result;
                        }

                        count = 1;
                        currentRun = game.Result;
                    }
                }

                calcResult.Add(new ConsecutiveResult(
                
                    Info: item.Info, ConsecutiveWins: numWins, ConsecutiveDraws: numDraws,
                    ConsecutiveLosses: numLosses, LosingStreak: nonWinningStreak, WinningStreak: nonLosingStreak
                ));
            }

            return calcResult;
        }

        private record ConsecutiveResult((long teamId, string teamName, string competitionName) Info,
            int ConsecutiveWins, int ConsecutiveDraws, int ConsecutiveLosses, int WinningStreak,
            int LosingStreak);

        private StatisticDto GetMostPointsStatistic(List<ResultMetrics> metrics, int pointsForWin)
        {
            var mostPoints = metrics.MaxBy(x => x.NumDraws + x.NumWins * pointsForWin);
            return new StatisticDto(
                "Points",
                "Most Points",
                mostPoints.First().NumDraws + mostPoints.First().NumWins * pointsForWin,
                GetTeamNames(mostPoints),
                GetCompetitionNames(mostPoints));
        }

        private StatisticDto GetFewestPointsStatistic(List<ResultMetrics> metrics, int pointsForWin)
        {
            var fewestPoints = metrics.MinBy(x => x.NumDraws + x.NumWins * pointsForWin);
            return new StatisticDto(
                "Points",
                "Fewest Points",
                fewestPoints.First().NumDraws + fewestPoints.First().NumWins * pointsForWin,
                GetTeamNames(fewestPoints),
                GetCompetitionNames(fewestPoints));
        }

        private StatisticDto GetBestPointsPerGameStatistic(List<ResultMetrics> metrics, int pointsForWin)
        {
            var fewestPoints = metrics.MaxBy(x =>
                (double)(x.NumDraws + x.NumWins * pointsForWin) / (x.NumWins + x.NumDraws + x.NumLosses));
            return new StatisticDto(
                "Points",
                "Best Points Per Game",
                (double)(fewestPoints.First().NumDraws + fewestPoints.First().NumWins * pointsForWin) /
                (fewestPoints.First().NumWins + fewestPoints.First().NumDraws + fewestPoints.First().NumLosses),
                GetTeamNames(fewestPoints),
                GetCompetitionNames(fewestPoints));
        }

        private StatisticDto GetMostGoalsStatistic(List<ResultMetrics> metrics)
        {
            var mostGoals = metrics.MaxBy(x => x.NumGoalsScored);
            return new StatisticDto(
                "Goals",
                "Most Goals",
                mostGoals.First().NumGoalsScored,
                GetTeamNames(mostGoals),
                GetCompetitionNames(mostGoals));
        }

        private StatisticDto GetFewestGoalsStatistic(List<ResultMetrics> metrics)
        {
            var fewestGoals = metrics.MinBy(x => x.NumGoalsScored);
            return new StatisticDto(
                "Goals",
                "Fewest Goals",
                fewestGoals.First().NumGoalsScored,
                GetTeamNames(fewestGoals),
                GetCompetitionNames(fewestGoals));
        }

        private StatisticDto GetBestGoalDifferenceStatistic(List<ResultMetrics> metrics)
        {
            var bestGoalDifference = metrics.MaxBy(x => x.NumGoalsScored - x.NumGoalsConceded);
            return new StatisticDto(
                "Goals",
                "Best Goal Difference",
                bestGoalDifference.First().NumGoalsScored - bestGoalDifference.First().NumGoalsConceded,
                GetTeamNames(bestGoalDifference),
                GetCompetitionNames(bestGoalDifference));
        }

        private StatisticDto GetBestGoalAverageStatistic(List<ResultMetrics> metrics)
        {
            var bestGoalAverage = metrics.MaxBy(x => (double)x.NumGoalsScored / x.NumGoalsConceded);
            return new StatisticDto(
                "Goals",
                "Best Goal Average",
                (double)bestGoalAverage.First().NumGoalsScored / bestGoalAverage.First().NumGoalsConceded,
                GetTeamNames(bestGoalAverage),
                GetCompetitionNames(bestGoalAverage));
        }

        private StatisticDto GetMostWinsStatistic(List<ResultMetrics> metrics)
        {
            var mostWins = metrics.MaxBy(x => x.NumWins);
            return new StatisticDto(
                "Results",
                "Most Wins",
                mostWins.First().NumWins,
                GetTeamNames(mostWins),
                GetCompetitionNames(mostWins));
        }

        private StatisticDto GetMostDrawsStatistic(List<ResultMetrics> metrics)
        {
            var mostDraws = metrics.MaxBy(x => x.NumDraws);
            return new StatisticDto(
                "Results",
                "Most Draws",
                mostDraws.First().NumDraws,
                GetTeamNames(mostDraws),
                GetCompetitionNames(mostDraws));
        }

        private StatisticDto GetMostLossesStatistic(List<ResultMetrics> metrics)
        {
            var mostLosses = metrics.MaxBy(x => x.NumLosses);
            return new StatisticDto(
                "Results",
                "Most Losses",
                mostLosses.First().NumLosses,
                GetTeamNames(mostLosses),
                GetCompetitionNames(mostLosses));
        }

        private static string GetTeamNames(IEnumerable<ResultMetrics> metrics)
        {
            return string.Join(",", metrics.Select(x => x.Info.teamName));
        }

        private static string GetCompetitionNames(IEnumerable<ResultMetrics> mostWins)
        {
            return string.Join(",", mostWins.Select(x => x.Info.competitionName));
        }
        
        private static string GetTeamNames(IEnumerable<ConsecutiveResult> metrics)
        {
            return string.Join(",", metrics.Select(x => x.Info.teamName));
        }

        private static string GetCompetitionNames(IEnumerable<ConsecutiveResult> mostWins)
        {
            return string.Join(",", mostWins.Select(x => x.Info.competitionName));
        }

        private List<ResultMetrics> CalculateResultMetrics(List<MatchModel> matches)
        {
            var homeGroup = matches
                .GroupBy(match => (teamId: match.HomeTeamId, teamName: match.HomeTeamName,
                    competitionName: match.CompetitionName))
                .Select(group => new ResultMetrics(
                    Info: group.Key,
                    NumWins: group.Count(m => m.HomeGoals > m.AwayGoals),
                    NumDraws: group.Count(m => m.HomeGoals == m.AwayGoals),
                    NumLosses: group.Count(m => m.HomeGoals < m.AwayGoals),
                    NumGoalsScored: group.Sum(m => m.HomeGoals),
                    NumGoalsConceded: group.Sum(m => m.AwayGoals)
                ));

            var awayGroup = matches
                .GroupBy(match => (teamId: match.AwayTeamId, teamName: match.AwayTeamName,
                    competitionName: match.CompetitionName))
                .Select(group => new ResultMetrics(
                    Info: group.Key,
                    NumWins: group.Count(m => m.AwayGoals > m.HomeGoals),
                    NumDraws: group.Count(m => m.AwayGoals == m.HomeGoals),
                    NumLosses: group.Count(m => m.AwayGoals < m.HomeGoals),
                    NumGoalsScored: group.Sum(m => m.AwayGoals),
                    NumGoalsConceded: group.Sum(m => m.HomeGoals)
                ));

            return homeGroup
                .Join(awayGroup,
                    home => home.Info.teamId,
                    away => away.Info.teamId,
                    (home, away) => new ResultMetrics(
                        Info: home.Info,
                        NumWins: home.NumWins + away.NumWins,
                        NumDraws: home.NumDraws + away.NumDraws,
                        NumLosses: home.NumLosses + away.NumLosses,
                        NumGoalsScored: home.NumGoalsScored + away.NumGoalsScored,
                        NumGoalsConceded: home.NumGoalsConceded + away.NumGoalsConceded
                    ))
                .ToList();
        }

        private record ResultMetrics((long teamId, string teamName, string competitionName) Info, int NumWins,
            int NumDraws, int NumLosses, int NumGoalsScored, int NumGoalsConceded);
    }
}