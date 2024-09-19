using System.Collections.Generic;
using System.Data;
using Dapper;
using Npgsql;

public class BaseContext
{
    private readonly IDbConnection _connection;

    public BaseContext(string connectionString)
    {
        _connection = new NpgsqlConnection(connectionString);
    }

    public IDbConnection Connection => _connection;

    public void OpenConnection()
    {
        if (_connection.State == ConnectionState.Closed)
        {
            _connection.Open();
        }
    }

    public void CloseConnection()
    {
        if (_connection.State == ConnectionState.Open)
        {
            _connection.Close();
        }
    }

    public IEnumerable<User> GetAllUsers()
    {
        OpenConnection();
        var users = _connection.Query<User>("SELECT * FROM Users");
        CloseConnection();
        return users;
    }
}
