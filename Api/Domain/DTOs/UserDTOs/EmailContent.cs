namespace Api.Domain;

public record EmailContent(
    string? To,
    string Subject,
    string Body
);