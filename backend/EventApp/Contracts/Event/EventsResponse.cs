namespace EventApp.Contracts;

public record EventsResponse(
      Guid Id,
      string Title,
      string Description,
      DateTime Date,
      Guid LocationId,
      Guid CategoryId,
      int MaxNumberOfMembers,
      int NumberOfMembers,
      string ImageUrl
  );