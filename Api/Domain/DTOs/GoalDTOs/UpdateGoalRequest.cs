namespace Api.Domain;

public record UpdateGoalRequest(
    decimal GoalAmount,
    DateTime Deadline,
    string Currency
);
