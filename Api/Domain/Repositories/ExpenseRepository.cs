namespace Api.Domain;

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

public class ExpenseRepository : IExpenseRepository
{
    private readonly IDbConnection _dbConnection;

    public ExpenseRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<bool> Save(Expense expense)
    {
        var query = "INSERT INTO expenses (id, user_id, currency, amount, description, category, date, created_at) " +
                    "VALUES (@Id, @UserId, @Currency, @Amount, @Description, @Category, @Date, @CreatedAt)";
        var result = await _dbConnection.ExecuteAsync(query, expense);
        return result > 0;
    }

    public async Task<bool> Delete(Expense expense)
    {
        var query = "DELETE FROM expenses WHERE id = @Id";
        var result = await _dbConnection.ExecuteAsync(query, new { expense.Id });
        return result > 0;
    }

    public async Task<bool> Update(Expense expense)
    {
        var query = "UPDATE expenses SET user_id = @UserId, currency = @Currency, amount = @Amount, " +
                    "description = @Description, category = @Category, date = @Date, created_at = @CreatedAt WHERE id = @Id";
        var result = await _dbConnection.ExecuteAsync(query, expense);
        return result > 0;
    }

    public async Task<Expense?> GetById(Guid id)
    {
        var query = "SELECT * FROM expenses WHERE id = @Id";
        return _dbConnection.QuerySingleOrDefault<Expense>(query, new { Id = id });
    }

    public async Task<List<Expense>> GetAll()
    {
        var query = "SELECT * FROM expenses";
        return _dbConnection.Query<Expense>(query).AsList();
    }

    public List<Expense> GetUserExpenseByDateRange(Guid userId, DateTime startDate, DateTime endDate)
    {
        var query = "SELECT * FROM expenses WHERE user_id = @UserId AND date BETWEEN @StartDate AND @EndDate";
        return _dbConnection.Query<Expense>(query, new { UserId = userId, StartDate = startDate, EndDate = endDate }).AsList();
    }

    public List<Expense> GetUserExpenseByCategory(Guid userId, string category)
    {
        var query = "SELECT * FROM expenses WHERE user_id = @UserId AND category = @Category";
        return _dbConnection.Query<Expense>(query, new { UserId = userId, Category = category }).AsList();
    }
    
    public async Task<List<Expense>> GetAllByUser(Guid userId)
    {
        var query = "SELECT * FROM expenses WHERE user_id = @UserId";
        return _dbConnection.Query<Expense>(query, new { UserId = userId }).AsList();
    }
}
