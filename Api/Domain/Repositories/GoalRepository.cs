namespace Api.Domain;

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

public class GoalRepository (IDbConnection connection) : IGoalRepository
{
    private readonly IDbConnection _dbConnection = connection;

    public async Task<bool> Save(Goal goal)
    {
        var query = @"INSERT INTO goals (id, user_id, currency, goal_amount, deadline, current_amount, created_at)
                    VALUES (@Id, @UserId, @Currency, @GoalAmount, @Deadline, @CurrentAmount, @CreatedAt)";

        var result = await _dbConnection.ExecuteAsync(query, new
        {
            goal.Id,
            UserId = goal.user_id,
            goal.Currency,
            GoalAmount = goal.goal_amount,
            goal.Deadline,
            goal.CurrentAmount,
            goal.CreatedAt
        });

        return result > 0;
    }

     public async Task<bool> Delete(Goal goal)
    {
        var query = "DELETE FROM goals WHERE id = @Id";
        var result = await _dbConnection.ExecuteAsync(query, new { goal.Id });
        return result > 0;
    }

    public async Task<bool> Update(Goal goal)
{
    var query = @"UPDATE goals 
                  SET goal_amount = @GoalAmount, 
                      deadline = @Deadline, 
                      currency = @Currency 
                  WHERE id = @Id";

    var result = await _dbConnection.ExecuteAsync(query, new
    {
        GoalAmount = goal.goal_amount,
        goal.Deadline,
        goal.Currency,
        goal.Id
    });

    return result > 0;
}


    public async Task<Goal?> GetById(Guid goalId){
        var query = "SELECT * FROM goals WHERE id = @Id";
        return await _dbConnection.QuerySingleOrDefaultAsync<Goal>(query, new { Id = goalId });
    }

    public async Task<List<Goal>> GetAll(){
        var query = "SELECT * FROM goals";
        return (await _dbConnection.QueryAsync<Goal>(query)).AsList();
    }

    public async Task<List<Goal>> GetActiveUserGoals(Guid userId)
    {
        var query = @"
            SELECT * 
            FROM goals 
            WHERE user_id = @UserId AND goal_amount != current_amount";
        
        return (await _dbConnection.QueryAsync<Goal>(query, new { UserId = userId })).AsList();
    }

    public async Task<List<Goal>> GetGoalsByUserId(Guid userId){
        var query = "SELECT * FROM goals WHERE user_id = @UserId";
        return (await _dbConnection.QueryAsync<Goal>(query, new { UserId = userId })).AsList();
    }
}