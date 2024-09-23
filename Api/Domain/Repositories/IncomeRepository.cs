namespace DefaultNamespace;

public interface IncomeRepository : IRepository<Income>
{
    List<Income> GetUserIncomeBySource(Guid userId, string source);
}