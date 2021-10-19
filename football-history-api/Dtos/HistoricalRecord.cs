using System;

namespace football.history.api.Dtos
{
    public record HistoricalRecord(long TeamId, HistoricalSeason[] HistoricalSeasons);

    public record HistoricalSeason(long SeasonId, int SeasonStartYear)
    {
        public int[] Boundaries { get; init; } = Array.Empty<int>();
        public HistoricalPosition? HistoricalPosition { get; init; }
    }

    public record HistoricalPosition(
        long CompetitionId,
        string CompetitionName,
        int Position,
        int OverallPosition,
        string? Status);
}