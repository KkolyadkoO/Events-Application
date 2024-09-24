namespace EventApp.Core.Models;

public class Event
{
    public Guid Id { get; }
    public string Title { get; } = string.Empty;
    public string Description { get; } = string.Empty;
    public DateTime Date { get; } = DateTime.Now;
    public Guid LocationId { get; }  
    public Guid CategoryId { get; }
    public int MaxNumberOfMembers { get; } = 0;
    public List<MemberOfEvent> Members { get; } = [];
    public string ImageUrl {  get; } = string.Empty;

    public Event(Guid id, string title, string description, DateTime date, Guid locationId, Guid categoryId, int maxNumberOfMembers, List<MemberOfEvent> members, string imageUrl)
    {
        Id = id;
        Title = title;
        Description = description;
        Date = date;
        LocationId = locationId;
        CategoryId = categoryId;
        MaxNumberOfMembers = maxNumberOfMembers;
        Members = members ?? new List<MemberOfEvent>();
        ImageUrl = imageUrl;
    }
    
}

