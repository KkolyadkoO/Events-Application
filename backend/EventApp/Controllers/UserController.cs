using EventApp.Application.DTOs.User;
using EventApp.Application.UseCases.RefreshToken;
using EventApp.Application.UseCases.User;
using EventApp.Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventApp.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly RegisterUserUseCase _registerUserUseCase;
    private readonly GetAllUsersUseCase _getAllUsersUseCase;
    private readonly DeleteRefreshToken _deleteRefreshToken;

    public UserController(RegisterUserUseCase registerUserUseCase,
        GetAllUsersUseCase getAllUsersUseCase,
        DeleteRefreshToken deleteRefreshToken)
    {
        _registerUserUseCase = registerUserUseCase;
        _getAllUsersUseCase = getAllUsersUseCase;
        _deleteRefreshToken = deleteRefreshToken;
    }
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegisterRequestDto request)
    {
        try
        {
            await _registerUserUseCase.Execute(request);
            return Ok();
        }
        catch (DuplicateUsers e)
        {
            return BadRequest(new { Message = e.Message });
        }
        
    }
    
    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetUsers()
    {
        var response = await _getAllUsersUseCase.Execute();
        return Ok(response);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        string? refreshToken = Request.Cookies["refresh_token"];
        if (refreshToken == null)
        {
            return NotFound(new { message = "Refresh token not found in cookies" });
        }

        try
        {
            await _deleteRefreshToken.Execute(refreshToken);
            Response.Cookies.Delete("refresh_token");
            return Ok();
        }
        catch (NotFoundException e)
        {
            return NotFound(new { message = e.Message });
        }
        
    }
}