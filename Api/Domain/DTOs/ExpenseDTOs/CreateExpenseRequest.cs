namespace Api.Domain;

public record CreateExpenseRequest(
    string Currency,
    decimal Amount,
    string Description,
    string Category,
    DateTime Date
);