namespace Api.Domain;

public record UpdateIncomeRequest(
    string Currency,
    decimal Amount,
    string Source,
    DateTime Date
);