namespace football.history.api.Models;

public record PointDeductionModel (
    long Id,
    long CompetitionId,
    int PointsDeducted,
    long TeamId,
    string TeamName,
    string Reason);