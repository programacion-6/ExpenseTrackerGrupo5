namespace Api.Domain;

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

public class IncomeRepository(IDbConnection connection) : IIncomeRepository
{
    private readonly IDbConnection _dbConnection = connection;

    public async Task<bool> Save(Income item)
    {
        var query = "INSERT INTO Incomes (Id, UserId, Currency, Amount, Source, Date, Created_At) VALUES (@Id, @UserId, @Currency, @Amount, @Source, @Date, @CreatedAt)";
        var result = await _dbConnection.ExecuteAsync(query, item);
        return result > 0;
    }

    public async Task<bool> Delete(Income item)
    {
        var query = "DELETE FROM Incomes WHERE Id = @Id";
        var result = _dbConnection.Execute(query, new { item.Id });
        return result > 0;
    }
    
    public async Task<bool> Update(Income item)
    {
        var query = "UPDATE Incomes SET UserId = @UserId, Source = @Source, Amount = @Amount, Date = @Date, currency = @Currency WHERE Id = @Id";
        var result = await _dbConnection.ExecuteAsync(query, item);
        return result > 0;
    }

    public async Task<Income?> GetById(Guid itemId)
    {
        var query = "SELECT * FROM Incomes WHERE Id = @Id";
        return await _dbConnection.QuerySingleOrDefaultAsync<Income>(query, new { Id = itemId });
    }

    public async Task<List<Income>> GetAll()
    {
        var query = "SELECT * FROM Incomes";
        return (await _dbConnection.QueryAsync<Income>(query)).AsList();
    }

    public List<Income> GetUserIncomeBySource(Guid userId, string source)
    {
        var query = "SELECT * FROM Incomes WHERE UserId = @UserId AND Source = @Source";
        return _dbConnection.Query<Income>(query, new { UserId = userId, Source = source }).AsList();
    }

    public async Task<List<Income>> GetIncomesByUserId(Guid userId)
    {
        var query = "SELECT * FROM Incomes WHERE UserId = @UserId";
        return (await _dbConnection.QueryAsync<Income>(query, new { UserId = userId })).AsList();
    }
}
