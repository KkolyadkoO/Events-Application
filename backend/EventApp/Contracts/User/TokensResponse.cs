namespace EventApp.Controllers;

public record TokensResponse
(
    string AccessToken,
    string RefreshToken
);