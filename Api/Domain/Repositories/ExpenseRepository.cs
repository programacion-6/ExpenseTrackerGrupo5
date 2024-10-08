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
        var query = "INSERT INTO expenses (id, userid, currency, amount, description, category, date, created_at) " +
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
        var query = "UPDATE expenses SET userid = @UserId, currency = @Currency, amount = @Amount, " +
                    "description = @Description, category = @Category, date = @Date, created_at = @CreatedAt WHERE id = @Id";
        var result = await _dbConnection.ExecuteAsync(query, expense);
        return result > 0;
    }

    public async Task<Expense?> GetById(Guid id)
    {
        var query = "SELECT * FROM expenses WHERE id = @Id";
        return await _dbConnection.QuerySingleOrDefaultAsync<Expense>(query, new { Id = id });
    }

    public async Task<List<Expense>> GetAll()
    {
        var query = "SELECT * FROM expenses";
        return _dbConnection.Query<Expense>(query).AsList();
    }

    public List<Expense> GetUserExpenseByDateRange(Guid userId, DateTime startDate, DateTime endDate)
    {
        var query = "SELECT * FROM expenses WHERE userid = @UserId AND date BETWEEN @StartDate AND @EndDate";
        return _dbConnection.Query<Expense>(query, new { UserId = userId, StartDate = startDate, EndDate = endDate }).AsList();
    }

    public List<Expense> GetUserExpenseByCategory(Guid userId, string category)
    {
        var query = "SELECT * FROM expenses WHERE userid = @UserId AND category = @Category";
        return _dbConnection.Query<Expense>(query, new { UserId = userId, Category = category }).AsList();
    }

    public async Task<List<Expense>> GetAllByUser(Guid userId)
    {
        var query = "SELECT * FROM expenses WHERE userid = @UserId";
        return _dbConnection.Query<Expense>(query, new { UserId = userId }).AsList();
    }

    public async Task<List<Expense>> GetAllUserExpensesByMonth(Guid userId, DateTime month)
    {
        var query = @"
        SELECT * 
        FROM expenses 
        WHERE userid = @UserId 
        AND EXTRACT(MONTH FROM date) = @Month 
        AND EXTRACT(YEAR FROM date) = @Year";

        var parameters = new
        {
            UserId = userId,
            Month = month.Month,
            Year = month.Year
        };

        return (await _dbConnection.QueryAsync<Expense>(query, parameters)).AsList();
    }

    
    public async Task<Guid?> GetUserIdByExpenseId(Guid expenseId)
    {
        var query = "SELECT userid FROM expenses WHERE id = @ExpenseId";
        return await _dbConnection.QuerySingleOrDefaultAsync<Guid?>(query, new { ExpenseId = expenseId });
    }
}
