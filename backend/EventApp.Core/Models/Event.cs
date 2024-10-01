namespace EventApp.Core.Models;

public class Event
{
    public Guid Id { get; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;
    public Guid LocationId { get; set; }
    public Guid CategoryId { get; set; }
    public int MaxNumberOfMembers { get; set; } = 0;
    public List<MemberOfEvent> Members { get; } = new List<MemberOfEvent>();
    public string? ImageUrl { get; set; }

    public Event(Guid id, string title, string description, DateTime date, Guid locationId,
        Guid categoryId, int maxNumberOfMembers, string? imageUrl)
    {
        Id = id;
        Title = title;
        Description = description;
        Date = date;
        LocationId = locationId;
        CategoryId = categoryId;
        MaxNumberOfMembers = maxNumberOfMembers;
        ImageUrl = imageUrl;
    }
    
    public Event() { }
    
}

