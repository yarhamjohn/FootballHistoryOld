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

            return statistics;
        }

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
            var fewestPoints = metrics.MaxBy(x => (double)(x.NumDraws + x.NumWins * pointsForWin) / (x.NumWins + x.NumDraws + x.NumLosses));
            return new StatisticDto(
                "Points",
                "Best Points Per Game",
                (double) (fewestPoints.First().NumDraws + fewestPoints.First().NumWins * pointsForWin) / (fewestPoints.First().NumWins + fewestPoints.First().NumDraws + fewestPoints.First().NumLosses),
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
            var bestGoalAverage = metrics.MaxBy(x => (double) x.NumGoalsScored / x.NumGoalsConceded);
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