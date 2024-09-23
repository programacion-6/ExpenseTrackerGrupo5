namespace Api.Domain;

public record UpdateRecurrentExpenseRequest(
    string Currency,
    decimal Amount,
    string Description,
    string Category,
    DateTime Date
);
