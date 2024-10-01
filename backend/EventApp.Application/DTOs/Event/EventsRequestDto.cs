namespace EventApp.Application.DTOs.Event;

public record EventsRequestDto(
    string Title,
    string Description,
    DateTime Date,
    Guid LocationId,
    int MaxNumberOfMembers,
    Guid CategoryId
    );