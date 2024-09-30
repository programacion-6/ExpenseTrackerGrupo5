namespace Api.Domain;

public record CreateGoalRequest(
    decimal goal_amount,
    DateTime Deadline,
    string Currency
);
