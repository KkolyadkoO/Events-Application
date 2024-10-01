namespace EventApp.Application.DTOs.User;

public record UsersResponseDto(
    Guid Id,
    string UserName,
    string UserEmail,
    string Role
);