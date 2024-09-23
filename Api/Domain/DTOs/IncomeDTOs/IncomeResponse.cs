namespace Api.Domain;

public record IncomeResponse(
    Guid Id,
    Guid UserId,
    string Currency,
    decimal Amount,
    string Source,
    DateTime Date,
    DateTime CreatedAt
);
