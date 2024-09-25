namespace Api.Domain;

public record UpdateExpenseRequest(
    string Currency,
    decimal Amount,
    string Description,
    string Category,
    DateTime Date
);