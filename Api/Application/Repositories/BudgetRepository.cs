using System.Data;

using Api.Domain;

using Dapper;

namespace Api.Application;

public class BudgetRepository : IBudgetRepository
{
    private readonly IDbConnection _connection;

    public BudgetRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<bool> Save(Budget budget)
    {
        try
        {
            var query = @"
                INSERT INTO budgets (id, user_id, month, currency, amount, current_amount) 
                VALUES (@Id, @UserId, @Month, @Currency, @Amount, @CurrentAmount)";

            await _connection.ExecuteAsync(query, budget);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> Update(Budget budget)
    {
        try
        {
            var query = @"
                UPDATE budgets 
                SET user_id = @UserId, month = @Month, currency = @Currency, amount = @Amount, current_amount = @CurrentAmount
                WHERE id = @Id";

            await _connection.ExecuteAsync(query, budget);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> Delete(Budget budget)
    {
        try
        {
            var query = "DELETE FROM budgets WHERE id = @Id";
            var affectedRows = await _connection.ExecuteAsync(query, new { Id = budget.Id });
            return affectedRows > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<List<Budget>> GetAll()
    {
        var query = @"
            SELECT 
                id AS Id, 
                user_id AS UserId, 
                month AS Month, 
                currency AS Currency, 
                amount AS Amount, 
                current_amount AS CurrentAmount 
            FROM budgets";

        var budgets = await _connection.QueryAsync<Budget>(query);
        return budgets.ToList();
    }

    public async Task<Budget?> GetById(Guid budgetId)
    {
        var query = @"
            SELECT 
                id AS Id, 
                user_id AS UserId, 
                month AS Month, 
                currency AS Currency, 
                amount AS Amount, 
                current_amount AS CurrentAmount 
            FROM budgets 
            WHERE id = @Id";

        return await _connection.QueryFirstOrDefaultAsync<Budget>(query, new { Id = budgetId });
    }

    public async Task<Budget?> GetUserBudgetByMonth(Guid userId, DateTime month)
    {
        var query = @"
        SELECT 
            id AS Id, 
            user_id AS UserId, 
            month AS Month, 
            currency AS Currency, 
            amount AS Amount, 
            current_amount AS CurrentAmount 
        FROM budgets 
        WHERE user_id = @UserId 
        AND EXTRACT(MONTH FROM month) = @Month 
        AND EXTRACT(YEAR FROM month) = @Year";

        return await _connection.QueryFirstOrDefaultAsync<Budget>(
            query,
            new { UserId = userId, Month = month.Month, Year = month.Year }
        );
    }


}
