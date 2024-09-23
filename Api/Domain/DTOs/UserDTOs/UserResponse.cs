namespace Api.Domain;

public record UserResponse(
    Guid Id,
    string Name,
    string Email,
    DateTime CreatedAt
);