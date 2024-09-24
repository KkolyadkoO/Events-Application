namespace EventApp.Controllers;

public record UserRegisterRequest(
    string Username,
    string UserEmail,
    string Password,
    string Role
    );