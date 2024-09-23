namespace Api.Domain;

public record UpdateBudgetRequest(
    string Currency,
    decimal Amount
);
