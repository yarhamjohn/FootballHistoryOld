namespace football.history.api.Repositories.Team
{
    public record PositionModel (
        long Id, 
        long CompetitionId, 
        string CompetitionName,
        long TeamId, 
        string TeamName,
        int Position, 
        string? Status);
}
