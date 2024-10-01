namespace EventApp.Application.DTOs.User;

public record UserLoginRequestDto
(
    string UserName,
    string Password
);