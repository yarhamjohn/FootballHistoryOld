namespace football.history.api.Domain;

public record CategorisedStatistics(string Category, Statistic[] Statistics);
    
public record Statistic(string Name, double Value, string TeamName, string CompetitionName);