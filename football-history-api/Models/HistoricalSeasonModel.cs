namespace football.history.api.Models;

public record HistoricalSeasonModel(
    long SeasonId,
    int StartYear,
    HistoricalPositionModel? PositionModel
);
    
public record HistoricalPositionModel(
    long CompetitionId,
    string CompetitionName,
    int Tier,
    int TotalPlaces,
    int? Position,
    string? Status
);