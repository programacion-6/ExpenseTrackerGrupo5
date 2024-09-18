namespace DefaultNamespace;

public interface IncomeRepository
{
    List<Income> GetByDateRange(DateTime startDate, DateTime endDate);
    List<Income> GetBySource(string source);
}