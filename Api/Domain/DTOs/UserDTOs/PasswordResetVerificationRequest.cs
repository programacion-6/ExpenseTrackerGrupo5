namespace Api.Domain;

public record PasswordResetVerificationRequest(
    string Code,
    string Email,
    string NewPassword
);