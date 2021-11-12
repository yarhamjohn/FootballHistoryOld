namespace football.history.api.Repositories;

public record PositionModel (
    long Id, 
    long CompetitionId, 
    string CompetitionName,
    long TeamId, 
    string TeamName,
    int LeaguePosition, 
    string? Status);