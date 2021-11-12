using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using football.history.api.Exceptions;

namespace football.history.api.Repositories.Team;

public interface IPositionRepository
{
    List<PositionModel> GetCompetitionPositions(long competitionId);
    List<PositionModel> GetTeamPositions(long teamId);
    PositionModel GetPosition(long competitionId, long teamId);
}

public class PositionRepository : IPositionRepository
{
    private readonly IDatabaseConnection _connection;
    private readonly IPositionCommandBuilder _queryBuilder;

    public PositionRepository(IDatabaseConnection connection, IPositionCommandBuilder queryBuilder)
    {
        _connection = connection;
        _queryBuilder = queryBuilder;
    }

    public List<PositionModel> GetCompetitionPositions(long competitionId)
    {
        _connection.Open();
        var cmd = _queryBuilder.Build(_connection, competitionId, null);
        var positions = GetPositionModels(cmd);
        _connection.Close();

        return positions;
    }

    public List<PositionModel> GetTeamPositions(long teamId)
    {
        _connection.Open();
        var cmd = _queryBuilder.Build(_connection, null, teamId);
        var positions = GetPositionModels(cmd);
        _connection.Close();

        return positions;
    }


    public PositionModel GetPosition(long competitionId, long teamId)
    {
        _connection.Open();
        var cmd = _queryBuilder.Build(_connection, competitionId, teamId);
        var positions = GetPositionModels(cmd);
        _connection.Close();

        return positions.Count switch
        {
            1 => positions.Single(),
            0 => throw new DataNotFoundException(
                $"No position was found for the specified competitionId ({competitionId}) and teamId ({teamId})."),
            _ => throw new DataInvalidException(
                $"{positions.Count} were found for the specified competitionId ({competitionId}) and teamId ({teamId}).")
        };
    }

    private static List<PositionModel> GetPositionModels(DbCommand cmd)
    {
        var positions = new List<PositionModel>();

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            positions.Add(GetPositionModel(reader));
        }

        return positions;
    }

    private static PositionModel GetPositionModel(DbDataReader reader)
        => new(
            Id: reader.GetInt64(0),
            CompetitionId: reader.GetInt64(1),
            CompetitionName: reader.GetString(2),
            TeamId: reader.GetInt64(3),
            TeamName: reader.GetString(4),
            Position: reader.GetByte(5),
            Status: reader.IsDBNull(6) ? null : reader.GetString(6));
}