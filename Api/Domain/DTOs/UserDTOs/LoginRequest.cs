namespace Api.Domain;

public record LoginRequest(
    string Email,
    string Password
);
