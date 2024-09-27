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
            goal.UserId,
            goal.Currency,
            goal.GoalAmount,
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

    public async Task<bool> Update(Goal goal){
        return true;
    }

    public async Task<Goal?> GetById(Guid goalId){
        return new Goal();
    }

    public async Task<List<Goal>> GetAll(){
        var query = "SELECT * FROM goals";
        return (await _dbConnection.QueryAsync<Goal>(query)).AsList();
    }

    public List<Goal> GetActiveUserGoals(Guid userId){
        return new List<Goal>();
    }
}