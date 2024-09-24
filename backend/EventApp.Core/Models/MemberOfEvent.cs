namespace EventApp.Core.Models;
public class MemberOfEvent
{
    public MemberOfEvent(Guid id, string name, string lastName, DateTime birthday, DateTime dateOfRegistration, string email,
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

    

    public Guid Id { get;  }
    public string Name { get; } = string.Empty;
    public string LastName { get;  } = string.Empty;
    public DateTime Birthday { get; } = DateTime.Today;
    public DateTime DateOfRegistration { get; } = DateTime.Now;
    public string Email { get; } = string.Empty;
    public Guid UserId { get; }
    public Guid EventId { get; } 

}