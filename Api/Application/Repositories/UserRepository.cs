using System.Data;

using Api.Domain;

using Dapper;

namespace Api.Application;

public class UserRepository : IUserRepository
{
    private readonly IDbConnection _connection;

    public UserRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<bool> Save(User user)
    {
        try
        {
            var query =
            "INSERT INTO users (id, name, email, password_hash, created_at) VALUES (@Id, @Name, @Email, @PasswordHash, @CreatedAt)";
            await _connection.ExecuteAsync(query, user);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> Delete(User user)
    {
        try
        {
            var query = "DELETE FROM Users WHERE id = @Id";
            var affectedRows = await _connection.ExecuteAsync(query, new { Id = user.Id });
            return affectedRows > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<List<User>> GetAll()
    {
        var query = @"
            SELECT 
                id AS Id, 
                name AS Name, 
                email AS Email, 
                password_hash AS PasswordHash, 
                created_at AS CreatedAt

            FROM Users";
        var users = await _connection.QueryAsync<User>(query);
        return users.ToList();
    }

    public async Task<User?> GetByEmail(string email)
    {
        var query = @"
            SELECT 
                id AS Id, 
                name AS Name, 
                email AS Email, 
                password_hash AS PasswordHash, 
                created_at AS CreatedAt 
            FROM users 
            WHERE email = @Email";

        return await _connection.QueryFirstOrDefaultAsync<User>(
            query,
            new { Email = email.ToLower() }
        );
    }

    public Task<User?> GetById(Guid itemId)
    {
        string sql = @"
            SELECT 
                id AS Id, 
                name AS Name, 
                email AS Email, 
                password_hash AS PasswordHash, 
                created_at AS CreatedAt 
            FROM Users WHERE id = @Id";
        return _connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = itemId });
    }

    public async Task<bool> Update(User user)
    {
        try
        {
            var query =
                        "UPDATE Users SET name = @Name, email = @Email, password_hash = @PasswordHash WHERE id = @Id";
            await _connection.ExecuteAsync(query, user);
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
}