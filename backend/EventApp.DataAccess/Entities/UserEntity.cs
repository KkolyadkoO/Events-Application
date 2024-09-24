namespace EventApp.DataAccess.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty ;
    public string UserEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "user";
    public List<MemberOfEventEntity> MemberOfEvents { get; set; } = [];
}
