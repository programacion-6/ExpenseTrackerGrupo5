namespace DefaultNamespace;

public class IExpenseRepository
{
    List<Expense> GetByDateRange(DateTime startDate, DateTime endDate);
    List<Expense> GetByCategory(string category);
    List<Expense> GetRecurrentExpenses();
}