using EventApp.Core.Models;

namespace EventApp.DataAccess.Entities;

public class MemberOfEventEntity
{ 
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime Birthday { get; set; } = DateTime.Now;
    public DateTime DateOfRegistration { get; set; } = DateTime.Now;
    public string Email { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public Guid EventId { get; set; }



}