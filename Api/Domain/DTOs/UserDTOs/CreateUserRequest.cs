namespace Api.Domain;

public record CreateUserRequest(
    string Name,
    string Email,
    string PasswordHash
);