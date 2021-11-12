using System;

namespace football.history.api.Domain;

public record Match (
    long Id,
    DateTime MatchDate,
    MatchCompetition Competition,
    MatchRules Rules,
    MatchTeam HomeTeam,
    MatchTeam AwayTeam);

public record MatchRules(
    string Type,
    string? Stage,
    bool ExtraTime,
    bool Penalties,
    int? NumLegs,
    bool AwayGoals,
    bool Replays);

public record MatchTeam(
    long Id,
    string Name,
    string Abbreviation,
    int Goals,
    int GoalsExtraTime,
    int PenaltiesTaken,
    int PenaltiesScored);
        
public record MatchCompetition(
    long Id, 
    string Name,
    int StartYear,
    int EndYear,
    string Level);