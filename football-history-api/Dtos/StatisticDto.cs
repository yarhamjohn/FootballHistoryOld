namespace football.history.api.Builders.Team
{
    public record StatisticDto(
        string Category, string Name, double Value, string TeamName, string CompetitionName);
}