using System.Data.Common;
using football.history.api.Models;
using Microsoft.Data.SqlClient;

namespace football.history.api.Repositories;

public interface IPointDeductionRepository
{
    /// <summary>
    /// Retrieves models for all point deductions imposed for the
    /// provided <paramref name="competitionId" />.
    /// </summary>
    ///
    /// <param name="competitionId">
    /// The id of the required competition.
    /// </param>
    /// 
    /// <returns>
    /// A collection of <see cref="PointDeductionModel">PointDeductionModels</see>
    /// for the given <paramref name="competitionId" />.
    /// Can be empty if the <paramref name="competitionId"/> matched no competition
    /// or there were no point deductions in the competition.
    /// </returns>
    PointDeductionModel[] GetPointDeductions(long competitionId);
}

public class PointDeductionRepository : IPointDeductionRepository
{
    private readonly IDatabaseConnection _connection;

    public PointDeductionRepository(IDatabaseConnection connection)
    {
        _connection = connection;
    }

    public PointDeductionModel[] GetPointDeductions(long competitionId)
    {
        _connection.Open();
        
        var cmd = BuildCommand(_connection, competitionId);
        var pointsDeductions = GetPointDeductionModels(cmd);
        
        _connection.Close();

        return pointsDeductions;
    }

    private static PointDeductionModel[] GetPointDeductionModels(DbCommand cmd)
    {
        var pointDeductions = new List<PointDeductionModel>();

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var pointDeductionModel = GetPointDeductionModel(reader);
            pointDeductions.Add(pointDeductionModel);
        }

        return pointDeductions.ToArray();
    }

    private static PointDeductionModel GetPointDeductionModel(DbDataReader reader)
    {
        return new(
            Id: reader.GetInt64(0),
            CompetitionId: reader.GetInt64(1),
            PointsDeducted: reader.GetInt16(2),
            TeamId: reader.GetInt64(3),
            TeamName: reader.GetString(4),
            Reason: reader.GetString(5));
    }

    private static DbCommand BuildCommand(IDatabaseConnection connection, long competitionId)
    {
        var cmd = connection.CreateCommand();
        cmd.CommandText = GetSql();

        AddParameters(cmd, competitionId);

        return cmd;
    }

    private static void AddParameters(DbCommand cmd, long competitionId)
    {
        cmd.Parameters.Add(
            new SqlParameter
            {
                ParameterName = "@CompetitionId",
                Value = competitionId
            });
    }

    private static string GetSql()
        => @"
            SELECT d.Id
                  ,d.CompetitionId
                  ,d.PointsDeducted
                  ,d.TeamId
                  ,t.Name
                  ,d.Reason
            FROM [dbo].[Deductions] AS d
            LEFT JOIN [dbo].[Teams] AS t ON t.Id = d.TeamId
            WHERE d.CompetitionId = @CompetitionId
            ";
}