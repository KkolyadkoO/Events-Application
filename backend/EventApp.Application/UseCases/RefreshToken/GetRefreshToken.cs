using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;

namespace EventApp.Application.UseCases.RefreshToken;

public class GetRefreshToken
{
    private readonly IUnitOfWork _unitOfWork;

    public GetRefreshToken(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Core.Models.RefreshToken> Execute(string refreshToken)
    {
        var foundedRefreshToken = await _unitOfWork.RefreshTokens.Get(refreshToken);
        if (foundedRefreshToken == null)
        {
            throw new NotFoundException($"Refresh token {refreshToken} not found");
        }
        return foundedRefreshToken;
    }
}