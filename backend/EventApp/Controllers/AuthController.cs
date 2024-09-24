using EventApp.Application;
using EventApp.Contracts;
using EventApp.Infrastructure;
using Microsoft.AspNetCore.Mvc;


namespace EventApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IUserService _userService;
    private readonly IRefreshTokenService _refreshTokenService;

    public AuthController(IJwtTokenService jwtTokenService, IUserService userService,
        IRefreshTokenService refreshTokenService)
    {
        _jwtTokenService = jwtTokenService;
        _userService = userService;
        _refreshTokenService = refreshTokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest model)
    {
        try
        {
            var user = await _userService.Login(model.UserName, model.Password);
            var usersResponse = new UsersResponse(user.Id, user.UserName, user.UserEmail, user.Role);
            var tokens = await _jwtTokenService.GenerateToken(user.Id, model.UserName, user.Role);
            HttpContext.Response.Cookies.Append("refresh_token", tokens.Item2);
            var tokensResponse = new TokensResponse(tokens.Item1, tokens.Item2);
            return Ok(new {usersResponse, tokensResponse});
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
    }

    [HttpGet("refresh")]
    public async Task<IActionResult> RefreshToken()
    {
        string? refreshToken = Request.Cookies["refresh_token"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized("Refresh token is empty");
        }
        var tokens = await _refreshTokenService.RefreshToken(refreshToken);
        HttpContext.Response.Cookies.Append("refresh_token", tokens.Item2);
        var tokensResponse = new TokensResponse(tokens.Item1, tokens.Item2);
        var rt = await _refreshTokenService.GetRefreshToken(refreshToken);
        var user = await _userService.GetUserById(rt.UserId);
        var usersResponse = new UsersResponse(user.Id, user.UserName, user.UserEmail, user.Role);

        return Ok(new {usersResponse, tokensResponse});
    }
}