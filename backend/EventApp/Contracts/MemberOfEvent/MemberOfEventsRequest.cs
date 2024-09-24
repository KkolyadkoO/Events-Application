namespace EventApp.Contracts;

public record MemberOfEventsRequest(
    string Name,
    string LastName,
    DateTime Birthday,
    string Email,
    Guid UserId,
    Guid EventId
);