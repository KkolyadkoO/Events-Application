namespace EventApp.Application.DTOs.MemberOfEvent;

public record MemberOfEventsRequestDto(
    string Name,
    string LastName,
    DateTime Birthday,
    string Email,
    Guid UserId,
    Guid EventId
);