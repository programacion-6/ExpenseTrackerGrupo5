namespace Api.Domain;

public interface IIncomeRepository : IRepository<Income>
{
    List<Income> GetUserIncomeBySource(Guid userId, string source);
    Task<List<Income>> GetIncomesByUserId(Guid userId);
    Task<List<Income>> GetAllUserIncomesByMonth(Guid userId, DateTime month);
}