using EventApp.Application.UseCases.RefreshToken;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.RefreshToken;

public class DeleteRefreshTokenTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteRefreshToken _deleteRefreshToken;

    public DeleteRefreshTokenTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _deleteRefreshToken = new DeleteRefreshToken(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldDeleteRefreshToken_WhenTokenExists()
    {
        // Arrange
        var token = "valid-refresh-token";
        var refreshToken = new Core.Models.RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Token = token,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        _unitOfWorkMock
            .Setup(uow => uow.RefreshTokens.Get(token))
            .ReturnsAsync(refreshToken);

        _unitOfWorkMock
            .Setup(uow => uow.RefreshTokens.Delete(token))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(uow => uow.Complete())
            .ReturnsAsync(1);

        // Act
        await _deleteRefreshToken.Execute(token);

        // Assert
        _unitOfWorkMock.Verify(uow => uow.RefreshTokens.Get(token), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.RefreshTokens.Delete(token), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenTokenDoesNotExist()
    {
        // Arrange
        var token = "non-existent-token";

        _unitOfWorkMock
            .Setup(uow => uow.RefreshTokens.Get(token))
            .ReturnsAsync((Core.Models.RefreshToken)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _deleteRefreshToken.Execute(token));
        Assert.Equal($"Refresh Token with token {token} not found", exception.Message);
        _unitOfWorkMock.Verify(uow => uow.RefreshTokens.Get(token), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.RefreshTokens.Delete(It.IsAny<string>()), Times.Never);
        _unitOfWorkMock.Verify(uow => uow.Complete(), Times.Never);
    }
}