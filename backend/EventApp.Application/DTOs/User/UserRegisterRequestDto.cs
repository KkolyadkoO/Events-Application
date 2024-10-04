namespace EventApp.Application.DTOs.User;

public record UserRegisterRequestDto(
    string UserName,
    string UserEmail,
    string Password,
    string Role
    );