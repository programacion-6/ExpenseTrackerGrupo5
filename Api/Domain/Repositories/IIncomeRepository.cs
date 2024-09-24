namespace Api.Domain;

public interface IIncomeRepository : IRepository<Income>
{
    List<Income> GetUserIncomeBySource(Guid userId, string source);
}