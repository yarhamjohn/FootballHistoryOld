using System;
using JetBrains.Annotations;

namespace football.history.api.Dtos
{
    [UsedImplicitly]
    public record HistoricalRecord(long TeamId, HistoricalSeason[] HistoricalSeasons);

    [UsedImplicitly]
    public record HistoricalSeason(long SeasonId, int SeasonStartYear)
    {
        public int[] Boundaries { [UsedImplicitly] get; init; } = Array.Empty<int>();
        public HistoricalPosition? HistoricalPosition { [UsedImplicitly] get; init; }
    }

    [UsedImplicitly]
    public record HistoricalPosition(
        long CompetitionId,
        string CompetitionName,
        int Position,
        int OverallPosition,
        string? Status);
}