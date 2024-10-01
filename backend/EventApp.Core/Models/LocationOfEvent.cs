namespace EventApp.Core.Models;

public class LocationOfEvent
{
    public LocationOfEvent(Guid id, string title)
    {
        Id = id;
        Title = title;
    }

    public LocationOfEvent() {}
    public Guid Id { get; set; }
    public string Title { get; set; }
}