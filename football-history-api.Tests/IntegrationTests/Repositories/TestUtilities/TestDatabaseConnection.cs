using System;
using System.Data.Common;
using System.IO;
using football.history.api.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace football.history.api.Tests.IntegrationTests.Repositories.TestUtilities;

public class TestDatabaseConnection : IDatabaseConnection
{
    private readonly SqlConnection _conn;
    private readonly string _databaseName;

    public TestDatabaseConnection(string databaseName)
    {
        _databaseName = databaseName;

        var username = Environment.GetEnvironmentVariable("TEST_DATABASE_USERNAME");
        var password = Environment.GetEnvironmentVariable("TEST_DATABASE_PASSWORD");
        _conn = new SqlConnection($"Server=localhost;User={username};Password={password}");
    }
        
    public void Open()
    {
        try
        {
            _conn.Open();
            _conn.ChangeDatabase(_databaseName);
        }
        catch (InvalidOperationException)
        {
            Close();
            Open();
        }
    }

    public void Close()
    {
        _conn.Close();
    }

    public DbCommand CreateCommand()
    {
        return _conn.CreateCommand();
    }

    public void CreateDatabase()
    {
        _conn.Open();

        CreateTestDatabase();
        PopulateTestDatabase();
            
        Close();
    }

    private void PopulateTestDatabase()
    {
        _conn.ChangeDatabase(_databaseName);

        var script = File.ReadAllText(@"./TestSource/Data.sql");
        var server = new Server(new ServerConnection(_conn));
        server.ConnectionContext.ExecuteNonQuery(script);
    }

    private void CreateTestDatabase()
    {
        DropTestDatabaseIfExists();

        var cmd = CreateCommand();
        cmd.CommandText = $@"
                    IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{_databaseName}')
                    BEGIN
                        CREATE DATABASE {_databaseName};
                    END;";
        cmd.ExecuteNonQuery();
    }

    public void DropDatabase()
    {
        _conn.Open();

        DropTestDatabaseIfExists();

        _conn.Close();
    }

    private void DropTestDatabaseIfExists()
    {
        var cmd = _conn.CreateCommand();
        cmd.CommandText = $@"
                    IF EXISTS (SELECT * FROM sys.databases WHERE name = '{_databaseName}')
                    BEGIN
                        DROP DATABASE {_databaseName};
                    END;";
        cmd.ExecuteNonQuery();
    }
}