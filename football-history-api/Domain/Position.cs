namespace football.history.api.Domain;

public record Position(
    long Id, 
    long CompetitionId, 
    string CompetitionName, 
    long TeamId, 
    string TeamName,
    int LeaguePosition, 
    string? Status);