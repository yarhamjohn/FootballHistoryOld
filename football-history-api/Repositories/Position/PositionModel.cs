namespace football.history.api.Repositories.Team
{
    public record PositionModel (long Id, long CompetitionId, long TeamId, int Position, string? Status);
}
