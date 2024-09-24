namespace EventApp.Core.Models;

public class RefreshToken
{
    public RefreshToken(Guid id, Guid userId, string token, DateTime expires)
    {
        Id = id;
        UserId = userId;
        Token = token;
        Expires = expires;
    }
    

    public Guid Id { get;  }
    public Guid UserId { get; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
}
