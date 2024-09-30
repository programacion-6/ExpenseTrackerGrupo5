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

    public async Task<ExpenseInsightsResponse> GetUserExpenseInsightsResponse(Guid userId)
    {
        var allUserExpenses = await _expenseRepository.GetAllByUser(userId);
        var totalExpenses = GetTotalExpenses(allUserExpenses);
        var highestSpendingCategory = GetHighestSpendingCategory(allUserExpenses);
        var mostExpensiveMonth = GetHighestSpendingMonth(allUserExpenses);
        var currency = allUserExpenses.Any() ? allUserExpenses[0].Currency : "";

        var expenseInsights = new ExpenseInsightsResponse(
            totalExpenses,
            highestSpendingCategory,
            mostExpensiveMonth,
            currency
        );

        return expenseInsights;
    }

    public async Task<MonthlySummaryResponse> GetUserMonthlySummary(Guid userId, DateTime month)
    {
        var allUserIncomes = await _incomeRepository.GetAllUserIncomesByMonth(userId, month);
        var allUserExpenses = await _expenseRepository.GetAllUserExpensesByMonth(userId, month);
        var budget = await _budgetService.GetUserBudgetByMonthOrCreate(userId, month);

        var totalIncome = GetTotalOfIncomes(allUserIncomes);
        var remainingBudget = budget.CurrentAmount;
        var highestSpendingCategory = GetHighestSpendingCategory(allUserExpenses);
        var currency = budget.Currency;
        var totalExpenses = GetTotalExpenses(allUserExpenses);

        var monthlySummary = new MonthlySummaryResponse(
                                 totalIncome,
                                 totalExpenses,
                                 remainingBudget,
                                 highestSpendingCategory,
                                 currency
                             );

        return monthlySummary;
    }

    private decimal GetTotalExpenses(List<Expense> allUserExpenses)
    {
        var initTotalValue = 0m;
        var totalExpenses = allUserExpenses.Aggregate(
                            initTotalValue, (acc, expense) => acc + expense.Amount);

        return totalExpenses;
    }

    private decimal GetTotalOfIncomes(List<Income> allUserIncomes)
    {
        var initTotalValue = 0m;
        var totalExpenses = allUserIncomes.Aggregate(
                            initTotalValue, (acc, income) => acc + income.Amount);

        return totalExpenses;
    }

    private string GetHighestSpendingCategory(List<Expense> allUserExpenses)
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

    private DateTime GetHighestSpendingMonth(List<Expense> allUserExpenses)
    {
        var expensesByMonth = new Dictionary<DateTime, decimal>();

        foreach (var expense in allUserExpenses)
        {
            var monthYear = new DateTime(expense.Date.Year, expense.Date.Month, 1);

            if (expensesByMonth.ContainsKey(monthYear))
            {
                expensesByMonth[monthYear] += expense.Amount;
            }
            else
            {
                expensesByMonth[monthYear] = expense.Amount;
            }
        }

        var highestSpendingMonth = expensesByMonth
            .OrderByDescending(pair => pair.Value)
            .FirstOrDefault();

        return highestSpendingMonth.Key != default
            ? highestSpendingMonth.Key
            : DateTime.MinValue;
    }

}