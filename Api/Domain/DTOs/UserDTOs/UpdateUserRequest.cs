namespace Api.Domain;

public record UpdateUserRequest(
    string Name,
    string Email,
    string PasswordHash
);