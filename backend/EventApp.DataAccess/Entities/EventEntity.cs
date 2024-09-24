namespace EventApp.DataAccess.Entities;

public class EventEntity
{
    public Guid Id { get; init; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;
    public Guid LocationId { get; set; }
    public Guid CategoryId { get; set;}
    public int MaxNumberOfMembers { get; set; } = 0;
    public List<MemberOfEventEntity> Members { get; set; } = [];
    public string ImageUrl { get; set; } = string.Empty;
}
