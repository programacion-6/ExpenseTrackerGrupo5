using Api.Domain;

namespace Api.Application;

public class ReportService : IReportService
{
    private readonly IBudgetService _budgetService;
    private readonly IIncomeRepository _incomeRepository;
    private readonly IExpenseRepository _expenseRepository;

    public ReportService(IBudgetService budgetService, IIncomeRepository incomeRepository, IExpenseRepository expenseRepository)
    {
        _budgetService = budgetService;
        _incomeRepository = incomeRepository;
        _expenseRepository = expenseRepository;
    }

    public Task<ExpenseInsightsResponse> GetUserExpenseInsightsResponse(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<MonthlySummaryResponse> GetUserMonthlySummary(Guid userId, DateTime month)
    {
        decimal initTotalValue = 0;
        var allUserIncomes = await _incomeRepository.GetAllUserIncomesByMonth(userId, month);
        var allUserExpenses = await _expenseRepository.GetAllUserExpensesByMonth(userId, month);
        var budget = await _budgetService.GetUserBudgetByMonthOrCreate(userId, month);

        var totalIncome = allUserIncomes.Aggregate(
                            initTotalValue, (acc, income) => acc + income.Amount);
        var remainingBudget = budget.CurrentAmount;
        var highestSpendingCategory = FindHighestSpendingCategory(allUserExpenses);
        var currency = budget.Currency;
        var totalExpenses = allUserExpenses.Aggregate(
                            initTotalValue, (acc, expense) => acc + expense.Amount);

        var monthlySummary = new MonthlySummaryResponse(
                                 totalIncome,
                                 totalExpenses,
                                 remainingBudget,
                                 highestSpendingCategory,
                                 currency
                             );

        return monthlySummary;
    }

    private string FindHighestSpendingCategory(List<Expense> allUserExpenses)
    {
        var highestSpendingCategory = allUserExpenses
                        .GroupBy(expense => expense.Category)
                        .Select(group => new
                        {
                            Category = group.Key,
                            TotalAmount = group.Sum(expense => expense.Amount)
                        })
                        .OrderByDescending(x => x.TotalAmount)
                        .FirstOrDefault();

        return highestSpendingCategory?.Category ?? string.Empty;
    }

}