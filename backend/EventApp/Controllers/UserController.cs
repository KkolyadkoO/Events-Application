using EventApp.Application;
using EventApp.Contracts;
using EventApp.Core.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventApp.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IRefreshTokenService _refreshTokenService;

    public UserController(IUserService userService, IRefreshTokenService refreshTokenService)
    {
        _userService = userService;
        _refreshTokenService = refreshTokenService;
    }
    [HttpPost("register")]
    public async Task<IResult> RegisterUser([FromBody] UserRegisterRequest request)
    {
        try
        {
            await _userService.Register(request.Username,request.UserEmail, request.Password, request.Role);
            return Results.Ok();
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }
        
    }
    
    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.GetAllUsers();
        var response = users.Select(u => new UsersResponse(u.Id,
            u.UserName, u.UserEmail, u.Role));
        return Ok(response);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        string? refreshToken = Request.Cookies["refresh_token"];
        if (refreshToken == null)
        {
            return NotFound("Refresh token not found in cookies");
        }

        try
        {
            await _refreshTokenService.DeleteRefreshToken(refreshToken);
            Response.Cookies.Delete("refresh_token");
            return Ok();
        }
        catch (Exception e)
        {
            return NotFound("Refresh token not found in base");
        }
        
    }
}