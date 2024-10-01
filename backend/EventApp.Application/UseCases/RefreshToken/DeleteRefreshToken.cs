using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;

namespace EventApp.Application.UseCases.RefreshToken;

public class DeleteRefreshToken
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRefreshToken(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task Execute(string refreshToken)
    {
        var FoundedRefreshToken = await _unitOfWork.RefreshTokens.Get(refreshToken);
        if (FoundedRefreshToken == null)
        {
            throw new NotFoundException($"Refresh Token with token {refreshToken} not found");
        }

        await _unitOfWork.RefreshTokens.Delete(refreshToken);
        await _unitOfWork.Complete();
    }
}