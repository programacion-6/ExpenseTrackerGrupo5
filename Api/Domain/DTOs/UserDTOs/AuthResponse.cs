namespace Api.Domain;

public record AuthResponse(
    string Token,
    DateTime ExpiresAt
);