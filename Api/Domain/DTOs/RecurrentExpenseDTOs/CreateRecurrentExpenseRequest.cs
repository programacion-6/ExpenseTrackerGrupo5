namespace Api.Domain;

public record CreateRecurrentExpenseRequest(
    string Currency,
    decimal Amount,
    string Description,
    string Category,
    DateTime Date
);