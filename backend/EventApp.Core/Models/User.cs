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
    
    public User() { }
    
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty ;
    public string UserEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "user";
    public List<MemberOfEvent> MemberOfEvents { get; } = [];
}