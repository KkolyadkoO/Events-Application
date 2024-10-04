using EventApp.Application.Exceptions;
using EventApp.Application.UseCases.RefreshToken;
using EventApp.Core.Abstractions;
using EventApp.Core.Abstractions.Repositories;
using EventApp.DataAccess.Abstractions;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.RefreshToken;

public class GetRefreshTokenTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetRefreshToken _getRefreshToken;

    public GetRefreshTokenTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _getRefreshToken = new GetRefreshToken(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldReturnRefreshToken_WhenTokenExists()
    {
        var token = "valid-refresh-token";
        var refreshToken = new Core.Models.RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Token = token,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        _unitOfWorkMock
            .Setup(uow => uow.RefreshTokens.GetByTokenAsync(token))
            .ReturnsAsync(refreshToken);

        var result = await _getRefreshToken.Execute(token);

        Assert.NotNull(result);
        Assert.Equal(token, result.Token);
        _unitOfWorkMock.Verify(uow => uow.RefreshTokens.GetByTokenAsync(token), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenTokenDoesNotExist()
    {
        var token = "non-existent-token";

        _unitOfWorkMock
            .Setup(uow => uow.RefreshTokens.GetByTokenAsync(token))
            .ReturnsAsync((Core.Models.RefreshToken)null);

        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _getRefreshToken.Execute(token));
        Assert.Equal($"Refresh token {token} not found", exception.Message);
        _unitOfWorkMock.Verify(uow => uow.RefreshTokens.GetByTokenAsync(token), Times.Once);
    }
}