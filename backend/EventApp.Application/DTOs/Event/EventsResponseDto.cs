namespace EventApp.Application.DTOs.Event;

public record EventsResponseDto(
    Guid Id = default,
    string Title = "",
    string Description = "",
    DateTime Date = default,
    Guid LocationId = default,
    Guid CategoryId = default,
    int MaxNumberOfMembers = 0,
    int NumberOfMembers = 0,
    byte[] Image = null
);