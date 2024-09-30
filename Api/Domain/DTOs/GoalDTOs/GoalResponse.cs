namespace Api.Domain;

public record GoalResponse(
    Guid Id,
    Guid UserId,
    string Currency,
    decimal goal_amount,
    decimal CurrentAmount,
    DateTime Deadline,
    DateTime CreatedAt
);