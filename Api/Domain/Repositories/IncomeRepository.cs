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
        var query = "INSERT INTO Incomes (Id, UserId, Source, Amount, Date) VALUES (@Id, @UserId, @Source, @Amount, @Date)";
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
        var query = "UPDATE Incomes SET UserId = @UserId, Source = @Source, Amount = @Amount, Date = @Date WHERE Id = @Id";
        var result = _dbConnection.Execute(query, item);
        return result > 0;
    }

    public Income GetById(Guid itemId)
    {
        var query = "SELECT * FROM Incomes WHERE Id = @Id";
        return _dbConnection.QuerySingleOrDefault<Income>(query, new { Id = itemId });
    }

    public List<Income> GetAll()
    {
        var query = "SELECT * FROM Incomes";
        return _dbConnection.Query<Income>(query).AsList();
    }

    public List<Income> GetUserIncomeBySource(Guid userId, string source)
    {
        var query = "SELECT * FROM Incomes WHERE UserId = @UserId AND Source = @Source";
        return _dbConnection.Query<Income>(query, new { UserId = userId, Source = source }).AsList();
    }
}
