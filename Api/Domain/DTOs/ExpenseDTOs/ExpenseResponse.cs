namespace Api.Domain;

public record ExpenseResponse(
    Guid Id,
    Guid UserId,
    string Currency,
    decimal Amount,
    string Description,
    string Category,
    DateTime Date,
    DateTime CreatedAt
);