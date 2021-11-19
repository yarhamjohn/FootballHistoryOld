using System;
using System.Collections.Generic;
using System.Linq;
using football.history.api.Domain;
using football.history.api.Models;
using football.history.api.Repositories;
using max = MoreLinq.Extensions.MaxByExtension;
using min = MoreLinq.Extensions.MinByExtension;

namespace football.history.api.Builders;

public interface IStatisticsBuilder
{
    CategorisedStatistics[] BuildSeasonStatistics(long seasonId);
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

    public CategorisedStatistics[] BuildSeasonStatistics(long seasonId)
    {
        var matches = _matchRepository.GetMatches(
            competitionId: null,
            seasonId,
            teamId: null,
            MatchType.League,
            matchDate: null);

        var competitions = _competitionRepository.GetCompetitions(seasonId);
        var pointsForWin = competitions.Select(x => x.PointsForWin).Distinct().ToList();
        if (pointsForWin.Count != 1)
        {
            throw new InvalidOperationException(
                $"Expected points for win to be the same for all competitions in a single season ({seasonId})");
        }

        var statistics = new List<CategorisedStatistics>();

        var metrics = CalculateResultMetrics(matches);

        statistics.Add(new CategorisedStatistics("Points",
            new []
            {
                GetMostPointsStatistic(metrics, pointsForWin.Single()),
                GetFewestPointsStatistic(metrics, pointsForWin.Single()),
                GetBestPointsPerGameStatistic(metrics, pointsForWin.Single())
            }));
            
        statistics.Add(new CategorisedStatistics("Goals",
            new []
            {
                GetMostGoalsStatistic(metrics),
                GetFewestGoalsStatistic(metrics),
                GetBestGoalDifferenceStatistic(metrics),
                GetBestGoalAverageStatistic(metrics)
            }));

        var consecutiveMetrics = GetConsecutiveMetrics(matches);

        statistics.Add(new CategorisedStatistics("Results",
            new []
            {
                GetMostWinsStatistic(metrics),
                GetMostDrawsStatistic(metrics),
                GetMostLossesStatistic(metrics),
                GetMostConsecutiveWins(consecutiveMetrics),
                GetMostConsecutiveDraws(consecutiveMetrics),
                GetMostConsecutiveLosses(consecutiveMetrics),
                GetMostGamesWithoutDefeat(consecutiveMetrics),
                GetMostGamesWithoutWin(consecutiveMetrics)
            }));
            
        return statistics.ToArray();
    }

    private Statistic GetMostConsecutiveWins(List<ConsecutiveResult> metrics)
    {
        var mostConsecutiveWins = max.MaxBy(metrics, x => x.ConsecutiveWins);
        return new Statistic(
            "Most Consecutive Wins",
            mostConsecutiveWins.First().ConsecutiveWins,
            GetTeamNames(mostConsecutiveWins),
            GetCompetitionNames(mostConsecutiveWins));
    }

    private Statistic GetMostConsecutiveDraws(List<ConsecutiveResult> metrics)
    {
        var mostConsecutiveDraws = max.MaxBy(metrics, x => x.ConsecutiveDraws);
        return new Statistic(
            "Most Consecutive Draws",
            mostConsecutiveDraws.First().ConsecutiveDraws,
            GetTeamNames(mostConsecutiveDraws),
            GetCompetitionNames(mostConsecutiveDraws));
    }

    private Statistic GetMostConsecutiveLosses(List<ConsecutiveResult> metrics)
    {
        var mostConsecutiveLosses = max.MaxBy(metrics, x => x.ConsecutiveLosses);
        return new Statistic(
            "Most Consecutive Losses",
            mostConsecutiveLosses.First().ConsecutiveLosses,
            GetTeamNames(mostConsecutiveLosses),
            GetCompetitionNames(mostConsecutiveLosses));
    }

    private Statistic GetMostGamesWithoutDefeat(List<ConsecutiveResult> metrics)
    {
        var winningStreak = max.MaxBy(metrics, x => x.WinningStreak);
        return new Statistic(
            "Most Games Without Defeat",
            winningStreak.First().WinningStreak,
            GetTeamNames(winningStreak),
            GetCompetitionNames(winningStreak));
    }

    private Statistic GetMostGamesWithoutWin(List<ConsecutiveResult> metrics)
    {
        var losingStreak = max.MaxBy(metrics, x => x.LosingStreak);
        return new Statistic(
            "Most Games Without Win",
            losingStreak.First().LosingStreak,
            GetTeamNames(losingStreak),
            GetCompetitionNames(losingStreak));
    }

    private List<ConsecutiveResult> GetConsecutiveMetrics(MatchModel[] matches)
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
                    home.Info,
                    ResultSequence = home.ResultSequence.Concat(away.ResultSequence).OrderBy(x => x.Date)
                })
            .ToList();

        var calcResult = new List<ConsecutiveResult>();
        foreach (var item in combined)
        {
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
                            streakCount =
                                1 + count; // ensure a sequence of L D W results in a non-losing streak of 2 
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

    private Statistic GetMostPointsStatistic(List<ResultMetrics> metrics, int pointsForWin)
    {
        var mostPoints = max.MaxBy(metrics, x => x.NumDraws + x.NumWins * pointsForWin);
        return new Statistic(
            "Most Points",
            mostPoints.First().NumDraws + mostPoints.First().NumWins * pointsForWin,
            GetTeamNames(mostPoints),
            GetCompetitionNames(mostPoints));
    }

    private Statistic GetFewestPointsStatistic(List<ResultMetrics> metrics, int pointsForWin)
    {
        var fewestPoints = min.MinBy(metrics, x => x.NumDraws + x.NumWins * pointsForWin);
        return new Statistic(
            "Fewest Points",
            fewestPoints.First().NumDraws + fewestPoints.First().NumWins * pointsForWin,
            GetTeamNames(fewestPoints),
            GetCompetitionNames(fewestPoints));
    }

    private Statistic GetBestPointsPerGameStatistic(List<ResultMetrics> metrics, int pointsForWin)
    {
        var fewestPoints = max.MaxBy(metrics, x =>
            (double)(x.NumDraws + x.NumWins * pointsForWin) / (x.NumWins + x.NumDraws + x.NumLosses));
        return new Statistic(
            "Best Points Per Game",
            (double)(fewestPoints.First().NumDraws + fewestPoints.First().NumWins * pointsForWin) /
            (fewestPoints.First().NumWins + fewestPoints.First().NumDraws + fewestPoints.First().NumLosses),
            GetTeamNames(fewestPoints),
            GetCompetitionNames(fewestPoints));
    }

    private Statistic GetMostGoalsStatistic(List<ResultMetrics> metrics)
    {
        var mostGoals = max.MaxBy(metrics, x => x.NumGoalsScored);
        return new Statistic(
            "Most Goals",
            mostGoals.First().NumGoalsScored,
            GetTeamNames(mostGoals),
            GetCompetitionNames(mostGoals));
    }

    private Statistic GetFewestGoalsStatistic(List<ResultMetrics> metrics)
    {
        var fewestGoals = min.MinBy(metrics, x => x.NumGoalsScored);
        return new Statistic(
            "Fewest Goals",
            fewestGoals.First().NumGoalsScored,
            GetTeamNames(fewestGoals),
            GetCompetitionNames(fewestGoals));
    }

    private Statistic GetBestGoalDifferenceStatistic(List<ResultMetrics> metrics)
    {
        var bestGoalDifference = max.MaxBy(metrics, x => x.NumGoalsScored - x.NumGoalsConceded);
        return new Statistic(
            "Best Goal Difference",
            bestGoalDifference.First().NumGoalsScored - bestGoalDifference.First().NumGoalsConceded,
            GetTeamNames(bestGoalDifference),
            GetCompetitionNames(bestGoalDifference));
    }

    private Statistic GetBestGoalAverageStatistic(List<ResultMetrics> metrics)
    {
        var bestGoalAverage = max.MaxBy(metrics, x => (double)x.NumGoalsScored / x.NumGoalsConceded);
        return new Statistic(
            "Best Goal Average",
            (double)bestGoalAverage.First().NumGoalsScored / bestGoalAverage.First().NumGoalsConceded,
            GetTeamNames(bestGoalAverage),
            GetCompetitionNames(bestGoalAverage));
    }

    private Statistic GetMostWinsStatistic(List<ResultMetrics> metrics)
    {
        var mostWins = max.MaxBy(metrics, x => x.NumWins);
        return new Statistic(
            "Most Wins",
            mostWins.First().NumWins,
            GetTeamNames(mostWins),
            GetCompetitionNames(mostWins));
    }

    private Statistic GetMostDrawsStatistic(List<ResultMetrics> metrics)
    {
        var mostDraws = max.MaxBy(metrics, x => x.NumDraws);
        return new Statistic(
            "Most Draws",
            mostDraws.First().NumDraws,
            GetTeamNames(mostDraws),
            GetCompetitionNames(mostDraws));
    }

    private Statistic GetMostLossesStatistic(List<ResultMetrics> metrics)
    {
        var mostLosses = max.MaxBy(metrics, x => x.NumLosses);
        return new Statistic(
            "Most Losses",
            mostLosses.First().NumLosses,
            GetTeamNames(mostLosses),
            GetCompetitionNames(mostLosses));
    }

    private static string GetTeamNames(IEnumerable<ResultMetrics> metrics)
    {
        return string.Join(", ", metrics.Select(x => x.Info.teamName));
    }

    private static string GetCompetitionNames(IEnumerable<ResultMetrics> mostWins)
    {
        return string.Join(", ", mostWins.Select(x => x.Info.competitionName));
    }

    private static string GetTeamNames(IEnumerable<ConsecutiveResult> metrics)
    {
        return string.Join(", ", metrics.Select(x => x.Info.teamName));
    }

    private static string GetCompetitionNames(IEnumerable<ConsecutiveResult> mostWins)
    {
        return string.Join(", ", mostWins.Select(x => x.Info.competitionName));
    }

    private List<ResultMetrics> CalculateResultMetrics(MatchModel[] matches)
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