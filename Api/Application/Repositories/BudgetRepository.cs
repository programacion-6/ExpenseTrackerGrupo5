
using System.Data;

using Api.Domain;

namespace Api.Application;

public class BudgetRepository : IBudgetRepository
{
    private readonly IDbConnection _connection;

    public BudgetRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public Task<bool> Delete(Budget item)
    {
        throw new NotImplementedException();
    }

    public Task<List<Budget>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<Budget?> GetById(Guid itemId)
    {
        throw new NotImplementedException();
    }

    public Budget GetUserBudgetByMonth(Guid userId, DateTime month)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Save(Budget item)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update(Budget item)
    {
        throw new NotImplementedException();
    }

}