namespace Api.Domain;

public record MonthlySummaryResponse(
    decimal TotalIncome,
    decimal TotalExpenses,
    decimal RemainingBudget,
    string HighestSpendingCategory,
    string Currency
);
