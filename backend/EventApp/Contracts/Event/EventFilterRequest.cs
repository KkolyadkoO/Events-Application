namespace EventApp.Contracts;

public record EventFilterRequest(
    string? Title = null,
    Guid? LocationId = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    Guid? Category = null,
    Guid? UserId = null,
    int? Page = null,
    int? PageSize = null
);