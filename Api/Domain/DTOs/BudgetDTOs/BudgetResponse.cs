namespace Api.Domain;

public record BudgetResponse(
    Guid Id,
    Guid UserId,
    string Currency,
    decimal Amount,
    decimal CurrentAmount,
    DateTime Month
);