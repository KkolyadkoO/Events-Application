namespace EventApp.Application.DTOs.User;

public record TokensResponse
(
    string AccessToken,
    string RefreshToken
);