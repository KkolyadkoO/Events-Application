using EventApp.Application.DTOs.User;
using EventApp.Application.UseCases.RefreshToken;
using EventApp.Application.UseCases.User;
using EventApp.Core.Exceptions;
using EventApp.Infrastructure;
using Microsoft.AspNetCore.Mvc;


namespace EventApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly LoginUserUseCase _loginUserUseCase;
    private readonly GetUserByIdUseCase _getUserByIdUseCase;
    private readonly Refresh _refresh;
    private readonly GetRefreshToken _getRefreshToken;

    public AuthController(IJwtTokenService jwtTokenService,
        LoginUserUseCase loginUserUseCase,
        GetUserByIdUseCase getUserByIdUseCase,
        Refresh refresh,
        GetRefreshToken getRefreshToken)
    {
        _jwtTokenService = jwtTokenService;
        _loginUserUseCase = loginUserUseCase;
        _getUserByIdUseCase = getUserByIdUseCase;
        _refresh = refresh;
        _getRefreshToken = getRefreshToken;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequestDto request)
    {
        try
        {
            var user = await _loginUserUseCase.Execute(request);
            var tokens = await _jwtTokenService.GenerateToken(user.Id, user.UserName, user.Role);
            HttpContext.Response.Cookies.Append("refresh_token", tokens.Item2);
            var tokensResponse = new TokensResponse(tokens.Item1, tokens.Item2);
            return Ok(new { user, tokensResponse });
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

        try
        {
            var tokens = await _refresh.Execute(refreshToken);
            HttpContext.Response.Cookies.Append("refresh_token", tokens.Item2);
            var tokensResponse = new TokensResponse(tokens.Item1, tokens.Item2);
            var rt = await _getRefreshToken.Execute(refreshToken);
            var user = await _getUserByIdUseCase.Execute(rt.UserId);

            return Ok(new { user, tokensResponse });
        }
        catch (NotFoundException e)
        {
            return NotFound(new { Message = e.Message });
        }
        catch (InvalidRefreshToken e)
        {
            return BadRequest(new { Message = e.Message });
        }
    }
}