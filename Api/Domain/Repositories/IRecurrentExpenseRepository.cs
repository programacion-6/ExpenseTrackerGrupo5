namespace DefaultNamespace;

public interface IRecurrentExpenseRepository : IRepository<RecurrentExpense>
{
    List<RecurrentExpense> GetUserRecurrentExpensesByMonth(Guid userId, DateTime month);
}