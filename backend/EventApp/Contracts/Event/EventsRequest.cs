namespace EventApp.Contracts;

public record EventsRequest(
    string Title,
    string Description,
    DateTime Date,
    Guid LocationId,
    int maxNumberOfMembers,
    Guid CategoryId,
    string ImageUrl
    );