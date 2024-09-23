namespace Api.Domain;

public record ExpenseInsightsResponse(
    decimal TotalExpenses,
    string HighestSpendingCategory,
    DateTime MostExpensiveMonth,
    string Currency
);
