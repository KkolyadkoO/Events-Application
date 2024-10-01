using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;
using EventApp.Infrastructure;

namespace EventApp.Application.UseCases.RefreshToken;

public class Refresh
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;

    public Refresh(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<(string, string)> Execute(string refreshToken)
    {
        var storedRefreshToken = await _unitOfWork.RefreshTokens.Get(refreshToken);
        if (storedRefreshToken == null)
        {
            throw new NotFoundException($"Refresh Token with token {refreshToken} not found");
        }

        if (storedRefreshToken.Expires < DateTime.Now)
        {
            throw new InvalidRefreshToken("Invalid or expired refresh token");
        }

        var user = await _unitOfWork.Users.GetById(storedRefreshToken.UserId);
        if (user == null)
        {
            throw new NotFoundException($"User with id {storedRefreshToken.UserId} not found");
        }
        var tokens = await _jwtTokenService.GenerateToken(user.Id, user.UserName, user.Role);
        await _unitOfWork.Complete();

        return (tokens.accessToken, tokens.refreshToken);
    }
}