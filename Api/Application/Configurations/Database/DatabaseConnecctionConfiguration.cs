using Npgsql;

namespace Api.Application;

public static class DatabaseConnecctionConfiguration
{
    public static NpgsqlConnection ConfigDatabaseConnection(IServiceProvider _)
    {
        var conn = new NpgsqlConnection(DatabaseConstants.DatabaseUri);
        conn.Open();

        return conn;
    }
}