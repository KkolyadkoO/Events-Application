namespace EventApp.Controllers;

public record UserLoginRequest
(
    string UserName,
    string Password
);