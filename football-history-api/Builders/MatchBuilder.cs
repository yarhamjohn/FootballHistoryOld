using System;
using System.Linq;
using football.history.api.Domain;
using football.history.api.Models;
using football.history.api.Repositories;

namespace football.history.api.Builders
{
    public interface IMatchBuilder
    {
        /// <summary>
        /// Constructs a collection of all <see cref="Match"/> domain objects.
        /// </summary>
        /// 
        /// <returns>
        /// Returns a collection of all <see cref="Match"/> domain objects.
        /// </returns>
        Match[] BuildMatches(
            long? competitionId,
            long? seasonId,
            long? teamId,
            MatchType? type,
            DateTime? matchDate);

        /// <summary>
        /// Constructs a <see cref="Match"/> domain object.
        /// </summary>
        /// 
        /// <param name="matchId">
        /// The id of the required match.
        /// </param>
        /// 
        /// <exception cref="InvalidOperationException">
        /// Thrown if the given <paramref name="matchId"/> matched more than one match.
        /// </exception>
        ///
        /// <returns>
        /// Returns the matching <see cref="Match"/> domain object or
        /// null if the given <paramref name="matchId"/> matched no match.
        /// </returns>
        Match? BuildMatch(long matchId);
    }

    public class MatchBuilder : IMatchBuilder
    {
        private readonly IMatchRepository _repository;

        public MatchBuilder(IMatchRepository repository)
        {
            _repository = repository;
        }

        public Match[] BuildMatches(long? competitionId,
            long? seasonId,
            long? teamId,
            MatchType? type,
            DateTime? matchDate)
            => _repository.GetMatches(competitionId, seasonId, teamId, type, matchDate)
                .Select(ToDomain)
                .ToArray();

        public Match? BuildMatch(long matchId)
        {
            var model = _repository.GetMatch(matchId);
            return model is null ? null : ToDomain(model);
        }

        private static Match ToDomain(MatchModel model)
            =>
                new(model.Id,
                    model.MatchDate,
                    Competition: new(
                        model.CompetitionId,
                        model.CompetitionName,
                        model.CompetitionStartYear,
                        model.CompetitionEndYear,
                        model.CompetitionLevel),
                    Rules: new(
                        model.RulesType,
                        model.RulesStage,
                        model.RulesExtraTime,
                        model.RulesPenalties,
                        model.RulesNumLegs,
                        model.RulesAwayGoals,
                        model.RulesReplays),
                    HomeTeam: new(
                        model.HomeTeamId,
                        model.HomeTeamName,
                        model.HomeTeamAbbreviation,
                        model.HomeGoals,
                        model.HomeGoalsExtraTime,
                        model.HomePenaltiesTaken,
                        model.HomePenaltiesScored),
                    AwayTeam: new(
                        model.AwayTeamId,
                        model.AwayTeamName,
                        model.AwayTeamAbbreviation,
                        model.AwayGoals,
                        model.AwayGoalsExtraTime,
                        model.AwayPenaltiesTaken,
                        model.AwayPenaltiesScored));
    }
}