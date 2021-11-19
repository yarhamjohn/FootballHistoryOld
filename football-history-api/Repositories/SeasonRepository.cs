namespace football.history.api.Repositories;

public interface ISeasonRepository
{
    /// <summary>
    /// Retrieves models for all seasons in the database.
    /// </summary>
    /// 
    /// <returns>
    /// A collection of <see cref="SeasonModel">SeasonModels</see>
    /// for each season in the database.
    /// </returns>
    SeasonModel[] GetAllSeasons();
        
    /// <summary>
    /// Retrieves a model from the database for the season
    /// matching the provided <paramref name="seasonId"/>.
    /// </summary>
    /// 
    /// <param name="seasonId">
    /// The id of the required season.
    /// </param>
    /// 
    /// <exception cref="InvalidOperationException">
    /// Thrown if the given <paramref name="seasonId"/> matched more than one season.
    /// </exception>
    ///
    /// <returns>
    /// Returns the matching <see cref="SeasonModel"/> or null if
    /// the given <paramref name="seasonId"/> matched no season.
    /// </returns>
    SeasonModel? GetSeason(long seasonId);
}

public class SeasonRepository : ISeasonRepository
{
    private readonly IDatabaseConnection _connection;

    public SeasonRepository(IDatabaseConnection connection)
    {
        _connection = connection;
    }

    public SeasonModel[] GetAllSeasons()
    {
        _connection.Open();

        var cmd = BuildCommand(_connection);
        var seasons = GetSeasonModels(cmd);
            
        _connection.Close();

        return seasons;
    }

    public SeasonModel? GetSeason(long seasonId)
    {
        _connection.Open();
            
        var cmd = BuildCommand(_connection, seasonId);
        var seasons = GetSeasonModels(cmd);
            
        _connection.Close();

        return seasons.SingleOrDefault();
    }

    private static SeasonModel[] GetSeasonModels(DbCommand cmd)
    {
        var seasons = new List<SeasonModel>();

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            seasons.Add(GetSeasonModel(reader));
        }

        return seasons.ToArray();
    }

    private static SeasonModel GetSeasonModel(DbDataReader reader)
        => new(
            Id: reader.GetInt64(0),
            StartYear: reader.GetInt16(1),
            EndYear: reader.GetInt16(2));
        
    private static DbCommand BuildCommand(IDatabaseConnection connection, long? seasonId = null)
    {
        var cmd = connection.CreateCommand();
        cmd.CommandText = GetSql(seasonId);

        AddParameters(cmd, seasonId);
            
        return cmd;
    }
        
    private static void AddParameters(DbCommand cmd, long? seasonId)
    {
        if (seasonId is null)
        {
            return;
        }
            
        cmd.Parameters.Add(
            new SqlParameter
            {
                ParameterName = "@SeasonId",
                Value = seasonId
            });
    }
        
    private static string GetSql(long? seasonId)
        => $@"
                SELECT 
                       s.Id,
                       s.StartYear,
                       s.EndYear
                FROM [dbo].[Seasons] AS s
                {BuildWhereClause(seasonId)}
                ";
        
    private static string BuildWhereClause(long? teamId)
        => teamId is null ? string.Empty : "WHERE s.Id = @SeasonId";
}