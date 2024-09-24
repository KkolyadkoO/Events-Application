namespace EventApp.Contracts;

public record MemberOfEventsResponse(
    Guid Id,
    string Name,
    string LastName,
    DateTime Birthday,
    string Email,
    Guid UserId,
    Guid EventId
);