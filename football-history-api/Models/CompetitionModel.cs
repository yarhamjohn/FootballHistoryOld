namespace football.history.api.Models;

public record CompetitionModel (
    long Id,
    string Name,
    long SeasonId,
    int StartYear,
    int EndYear,
    int Tier,
    string? Region,
    string? Comment,
    int PointsForWin,
    int TotalPlaces,
    int PromotionPlaces,
    int RelegationPlaces,
    int PlayOffPlaces,
    int RelegationPlayOffPlaces,
    int ReElectionPlaces,
    int? FailedReElectionPosition)
{
    public readonly string Level = $@"{Tier}{Region}";
}
