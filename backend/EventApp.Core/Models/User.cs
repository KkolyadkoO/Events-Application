namespace EventApp.Core.Models;

public class User
{
    public User(Guid id, string userName, string userEmail, string password, string role)
    {
        Id = id;
        UserName = userName;
        UserEmail = userEmail;
        Password = password;
        Role = role;
    }
    public Guid Id { get; }
    public string UserName { get; } = string.Empty ;
    public string UserEmail { get; } = string.Empty;
    public string Password { get; } = string.Empty;
    public string Role { get; } = "user";
    public List<MemberOfEvent> MemberOfEvents { get; } = [];
}