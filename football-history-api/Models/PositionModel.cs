namespace football.history.api.Models;

public record PositionModel (
    long Id, 
    long CompetitionId, 
    string CompetitionName,
    long TeamId, 
    string TeamName,
    int LeaguePosition, 
    string? Status);