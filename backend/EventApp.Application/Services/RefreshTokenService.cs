using EventApp.Core.Abstractions;
using EventApp.Core.Exceptions;
using EventApp.Core.Models;
using EventApp.Infrastructure;

namespace EventApp.Application;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;

    public RefreshTokenService(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<(string, string)> RefreshToken(string refreshToken)
    {
        try
        {
            var storedRefreshToken = await _unitOfWork.RefreshTokens.Get(refreshToken);
            if (storedRefreshToken.Expires < DateTime.Now)
            {
                throw new InvalidRefreshToken("Invalid or expired refresh token");
            }

            var user = await _unitOfWork.Users.GetById(storedRefreshToken.UserId);
            var tokens = await _jwtTokenService.GenerateToken(user.Id, user.UserName, user.Role);
            await _unitOfWork.Complete();

            return (tokens.accessToken, tokens.refreshToken);
        }
        catch (Exception e)
        {
            throw new RefreshTokenNotFound(e.Message);
        }
    }

    public async Task<string> DeleteRefreshToken(string refreshToken)
    {
        try
        {
            return await _unitOfWork.RefreshTokens.Delete(refreshToken);
        }
        catch (Exception e)
        {
            throw new RefreshTokenNotFound(e.Message);
        }
    }

    public async Task<RefreshToken> GetRefreshToken(string refreshToken)
    {
        try
        {
            return await _unitOfWork.RefreshTokens.Get(refreshToken);
        }
        catch (Exception e)
        {
            throw new RefreshTokenNotFound(e.Message);
        }
    }
}