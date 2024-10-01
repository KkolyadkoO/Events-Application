namespace EventApp.Application.DTOs.MemberOfEvent;

public record MemberOfEventsResponseDto(
    Guid Id,
    string Name,
    string LastName,
    DateTime Birthday,
    string Email,
    Guid UserId,
    Guid EventId
);