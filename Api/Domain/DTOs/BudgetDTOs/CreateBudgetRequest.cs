namespace Api.Domain;

public record CreateBudgetRequest(
    string Currency,
    decimal Amount
);
