namespace Api.Domain;

public record GoalResponse(
    Guid Id,
    Guid UserId,
    string Currency,
    decimal GoalAmount,
    decimal CurrentAmount,
    DateTime Deadline,
    DateTime CreatedAt
);