namespace Api.Domain;

public record CreateGoalRequest(
    decimal GoalAmount,
    DateTime Deadline,
    string Currency
);
