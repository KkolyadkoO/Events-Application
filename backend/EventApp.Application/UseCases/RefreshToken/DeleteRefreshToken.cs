using EventApp.Application.Exceptions;
using EventApp.DataAccess.Abstractions;

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
        var FoundedRefreshToken = await _unitOfWork.RefreshTokens.GetByTokenAsync(refreshToken);
        if (FoundedRefreshToken == null)
        {
            throw new NotFoundException($"Refresh Token with token {refreshToken} not found");
        }

        await _unitOfWork.RefreshTokens.DeleteByTokenAsync(refreshToken);
        await _unitOfWork.Complete();
    }
}