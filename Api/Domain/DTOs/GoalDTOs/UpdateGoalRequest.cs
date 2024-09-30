namespace Api.Domain;

public record UpdateGoalRequest(
    decimal goal_amount,
    DateTime Deadline,
    string Currency
);
