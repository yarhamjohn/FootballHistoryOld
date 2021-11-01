using System.Collections.Generic;

namespace football.history.api.Builders
{
    public record StatisticsDto(string Category, List<StatisticDto> statistics);
    
    public record StatisticDto(string Name, double Value, string TeamName, string CompetitionName);
}