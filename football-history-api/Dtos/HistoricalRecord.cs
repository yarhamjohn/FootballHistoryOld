namespace football.history.api.Dtos
{
    public record HistoricalRecord(long TeamId, HistoricalSeason[] HistoricalSeasons);

    public record HistoricalSeason(
        long SeasonId, int SeasonStartYear, int[] Boundaries, HistoricalPosition? HistoricalPosition);

    public record HistoricalPosition(
        long CompetitionId, string CompetitionName, int Position, int OverallPosition, string? Status);
}