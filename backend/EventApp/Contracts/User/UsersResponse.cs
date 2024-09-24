namespace EventApp.Contracts;

public record UsersResponse(
    Guid Id,
    string UserName,
    string UserEmail,
    string Role
);