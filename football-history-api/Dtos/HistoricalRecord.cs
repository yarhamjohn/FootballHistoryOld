namespace football.history.api.Dtos
{
    public record HistoricalRecord(long TeamId, HistoricalSeason[] HistoricalSeasons);

    public record HistoricalSeason
    {
        public long SeasonId { get; set; }
        public int SeasonStartYear { get; set; }
        public int[] Boundaries { get; set; }
        public HistoricalPosition? HistoricalPosition { get; set; }
    }

    public record HistoricalPosition(
        long CompetitionId, string CompetitionName, int Position, int OverallPosition, string? Status);
}