namespace EventApp.Core.Models;

public class MemberOfEvent
{
    public MemberOfEvent(Guid id, string name, string lastName, DateTime birthday, DateTime dateOfRegistration,
        string email,
        Guid userId, Guid eventId)
    {
        Id = id;
        Name = name;
        LastName = lastName;
        Birthday = birthday;
        DateOfRegistration = dateOfRegistration;
        Email = email;
        UserId = userId;
        EventId = eventId;
    }

    public MemberOfEvent() { }

    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime Birthday { get; set; } = DateTime.Today;
    public DateTime DateOfRegistration { get; set; } = DateTime.Now;
    public string Email { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public Guid EventId { get; set; }
}