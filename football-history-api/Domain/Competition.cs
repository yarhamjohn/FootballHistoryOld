namespace football.history.api.Domain;

public record Competition (
    long Id,
    string Name,
    CompetitionSeason Season,
    string Level,
    string? Comment,
    CompetitionRules Rules);

public record CompetitionRules(
    int PointsForWin,
    int TotalPlaces,
    int PromotionPlaces,
    int RelegationPlaces,
    int PlayOffPlaces,
    int RelegationPlayOffPlaces,
    int ReElectionPlaces,
    int? FailedReElectionPosition);

public record CompetitionSeason
(
    long Id,
    int StartYear,
    int EndYear 
);