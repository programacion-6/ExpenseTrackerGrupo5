namespace Api.Domain;

public record DateRangeRequest(
    Guid UserId,
    DateTime StartDate,
    DateTime EndDate
);