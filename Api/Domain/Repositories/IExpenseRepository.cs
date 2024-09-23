namespace DefaultNamespace;

public interface IExpenseRepository : IRepository<Expense>
{
    List<Expense> GetUserExpenseByDateRange(Guid userId, DateTime startDate, DateTime endDate);
    List<Expense> GetUserExpenseByCategory(Guid userId, string category);
}