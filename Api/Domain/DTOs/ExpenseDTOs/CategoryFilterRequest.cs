namespace Api.Domain;


public record CategoryFilterRequest(

    Guid UserId,
    string Category
);