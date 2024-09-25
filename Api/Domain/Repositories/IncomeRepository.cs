namespace Api.Domain;

public interface IncomeRepository : IRepository<Income>
{
    List<Income> GetUserIncomeBySource(Guid userId, string source);
}