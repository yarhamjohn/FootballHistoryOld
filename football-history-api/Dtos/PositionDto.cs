namespace football.history.api.Dtos
{
    public record PositionDto(
        long Id, 
        long CompetitionId, 
        string CompetitionName, 
        long TeamId, 
        string TeamName,
        int Position, 
        string? Status);
}