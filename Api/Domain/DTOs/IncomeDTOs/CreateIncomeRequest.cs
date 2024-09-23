namespace Api.Domain;

public record CreateIncomeRequest(
    string Currency,
    decimal Amount,
    string Source,
    DateTime Date
);
