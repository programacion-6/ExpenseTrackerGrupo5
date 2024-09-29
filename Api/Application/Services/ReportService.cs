
using Api.Domain;
using Api.Domain.Services;

namespace Api.Application;

public class ReportService : IReportService
{
    private readonly IBudgetService _budgetService;
    private readonly IIncomeService _incomeService;

    public ReportService(IBudgetService budgetService, IIncomeService incomeService)
    {
        _budgetService = budgetService;
        _incomeService = incomeService;
    }

    public Task<ExpenseInsightsResponse> GetUserExpenseInsightsResponse(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<MonthlySummaryResponse> GetUserMonthlySummary(Guid userId)
    {
        decimal initTotalValue = 0;
        var allUserIncomes = await _incomeService.GetIncomesByUserIdAsync(userId);
        var currentBudget = await _budgetService.GetCurrentUserBudget(userId);
        var allUserExpenses = new List<Expense>();

        var totalIncome = allUserIncomes.Aggregate(
                            initTotalValue, (acc, income) => acc + income.Amount);
        var remainingBudget = currentBudget.Amount - currentBudget.CurrentAmount;
        var highestSpendingCategory = string.Empty;

        if (allUserExpenses.Count != 0) {
            highestSpendingCategory = allUserExpenses
                                        .GroupBy(expense => expense.Category)
                                        .OrderByDescending(group => group.Count())
                                        .Select(group => group.Key)
                                        .FirstOrDefault();
        }
        
        var currency = currentBudget.Currency;
        var totalExpenses = allUserExpenses.Aggregate(
                            initTotalValue, (acc, expense) => acc + expense.Amount);

        var monthlySummary = new MonthlySummaryResponse(
                                 totalIncome, 
                                 totalExpenses, 
                                 remainingBudget, 
                                 highestSpendingCategory ?? string.Empty, 
                                 currency
                             );

        return monthlySummary;
    }

}