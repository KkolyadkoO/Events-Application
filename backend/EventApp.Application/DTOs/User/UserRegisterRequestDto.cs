namespace EventApp.Application.DTOs.User;

public record UserRegisterRequestDto(
    string Username,
    string UserEmail,
    string Password,
    string Role
    );