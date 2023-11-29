namespace football.history.api.Repositories;

public interface ITeamRepository
{
    /// <summary>
    /// Retrieves models for all teams in the database.
    /// </summary>
    /// 
    /// <returns>
    /// A collection of <see cref="TeamModel">TeamModels</see>
    /// for each team in the database.
    /// </returns>
    TeamModel[] GetAllTeams();
        
    /// <summary>
    /// Retrieves a model from the database for the team
    /// matching the provided <paramref name="teamId"/>.
    /// </summary>
    /// 
    /// <param name="teamId">
    /// The id of the required team.
    /// </param>
    /// 
    /// <exception cref="InvalidOperationException">
    /// Thrown if the given <paramref name="teamId"/> matched more than one team.
    /// </exception>
    ///
    /// <returns>
    /// Returns the matching <see cref="TeamModel"/> or null if
    /// the given <paramref name="teamId"/> matched no team.
    /// </returns>
    TeamModel? GetTeam(long teamId);
}

public class TeamRepository : ITeamRepository
{
    private readonly IDatabaseConnection _connection;

    public TeamRepository(IDatabaseConnection connection)
    {
        _connection = connection;
    }

    public TeamModel[] GetAllTeams()
    {
        _connection.Open();
            
        var cmd = BuildCommand(_connection);
        var teams = GetTeamModels(cmd);
            
        _connection.Close();

        return teams;
    }

    public TeamModel? GetTeam(long teamId)
    {
        _connection.Open();
            
        var cmd = BuildCommand(_connection, teamId);
        var teams = GetTeamModels(cmd);
            
        _connection.Close();

        return teams.SingleOrDefault();
    }
        
    private static TeamModel[] GetTeamModels(DbCommand cmd)
    {
        var teams = new List<TeamModel>();

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            teams.Add(GetTeamModel(reader));
        }

        return teams.ToArray();
    }
        
    private static TeamModel GetTeamModel(DbDataReader reader)
        => new(
            Id: reader.GetInt64(0),
            Name: reader.GetString(1),
            Abbreviation: reader.GetString(2),
            Notes: reader.IsDBNull(3) ? null : reader.GetString(3));
        
    private static DbCommand BuildCommand(IDatabaseConnection connection, long? teamId = null)
    {
        var cmd = connection.CreateCommand();
        cmd.CommandText = GetSql(teamId);

        AddParameters(cmd, teamId);
            
        return cmd;
    }

    private static void AddParameters(DbCommand cmd, long? teamId)
    {
        if (teamId is null)
        {
            return;
        }
            
        cmd.Parameters.Add(
            new SqlParameter
            {
                ParameterName = "@TeamId",
                Value = teamId
            });
    }

    private static string GetSql(long? teamId)
        => $@"
                SELECT 
                       t.Id,
                       t.Name,
                       t.Abbreviation,
                       t.Notes
                FROM dbo.Teams AS t
                {BuildWhereClause(teamId)}
                ";
        
    private static string BuildWhereClause(long? teamId)
        => teamId is null ? string.Empty : "WHERE t.Id = @TeamId";
}