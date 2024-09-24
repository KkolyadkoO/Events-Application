namespace EventApp.Core.Models;

public class LocationOfEvent
{
    public LocationOfEvent(Guid id, string title)
    {
        Id = id;
        Title = title;
    }

    public Guid Id { get; }
    public string Title { get; }
}