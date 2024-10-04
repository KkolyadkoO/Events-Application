using EventApp.Application.Exceptions;
using EventApp.Application.UseCases.RefreshToken;
using EventApp.Core.Abstractions;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Models;
using EventApp.DataAccess.Abstractions;
using EventApp.Infrastructure;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.RefreshToken;

public class RefreshTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
    private readonly Refresh _refreshUseCase;

    public RefreshTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _jwtTokenServiceMock = new Mock<IJwtTokenService>();
        _refreshUseCase = new Refresh(_unitOfWorkMock.Object, _jwtTokenServiceMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldReturnNewTokens_WhenRefreshTokenIsValid()
    {
        var token = "valid-refresh-token";
        var userId = Guid.NewGuid();
        var userName = "testuser";
        var role = "User";

        var refreshToken = new Core.Models.RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = token,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        var user = new Core.Models.User
        {
            Id = userId,
            UserName = userName,
            Role = role
        };

        var newAccessToken = "new-access-token";
        var newRefreshToken = "new-refresh-token";

        _unitOfWorkMock
            .Setup(uow => uow.RefreshTokens.GetByTokenAsync(token))
            .ReturnsAsync(refreshToken);

        _unitOfWorkMock
            .Setup(uow => uow.Users.GetByIdAsync(userId))
            .ReturnsAsync(user);

        _jwtTokenServiceMock
            .Setup(jwt => jwt.GenerateToken(user.Id, user.UserName, user.Role))
            .ReturnsAsync((newAccessToken, newRefreshToken));

        _unitOfWorkMock
            .Setup(uow => uow.Complete())
            .ReturnsAsync(1);

        var result = await _refreshUseCase.Execute(token);

        Assert.NotNull(result);
        Assert.Equal(newAccessToken, result.Item1);
        Assert.Equal(newRefreshToken, result.Item2);

        _unitOfWorkMock.Verify(uow => uow.RefreshTokens.GetByTokenAsync(token), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Users.GetByIdAsync(userId), Times.Once);
        _jwtTokenServiceMock.Verify(jwt => jwt.GenerateToken(user.Id, user.UserName, user.Role), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenRefreshTokenDoesNotExist()
    {
        var token = "non-existent-token";

        _unitOfWorkMock
            .Setup(uow => uow.RefreshTokens.GetByTokenAsync(token))
            .ReturnsAsync((Core.Models.RefreshToken)null);

        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _refreshUseCase.Execute(token));
        Assert.Equal($"Refresh Token with token {token} not found", exception.Message);

        _unitOfWorkMock.Verify(uow => uow.RefreshTokens.GetByTokenAsync(token), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Users.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _jwtTokenServiceMock.Verify(jwt => jwt.GenerateToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
        _unitOfWorkMock.Verify(uow => uow.Complete(), Times.Never);
    }

    [Fact]
    public async Task Execute_ShouldThrowInvalidRefreshToken_WhenTokenIsExpired()
    {
        var token = "expired-token";
        var userId = Guid.NewGuid();

        var refreshToken = new Core.Models.RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = token,
            Expires = DateTime.UtcNow.AddDays(-1)
        };

        _unitOfWorkMock
            .Setup(uow => uow.RefreshTokens.GetByTokenAsync(token))
            .ReturnsAsync(refreshToken);

        var exception = await Assert.ThrowsAsync<InvalidRefreshToken>(() => _refreshUseCase.Execute(token));
        Assert.Equal("Invalid or expired refresh token", exception.Message);

        _unitOfWorkMock.Verify(uow => uow.RefreshTokens.GetByTokenAsync(token), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Users.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _jwtTokenServiceMock.Verify(jwt => jwt.GenerateToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
        _unitOfWorkMock.Verify(uow => uow.Complete(), Times.Never);
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        var token = "valid-token-with-nonexistent-user";
        var userId = Guid.NewGuid();

        var refreshToken = new Core.Models.RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = token,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        _unitOfWorkMock
            .Setup(uow => uow.RefreshTokens.GetByTokenAsync(token))
            .ReturnsAsync(refreshToken);

        _unitOfWorkMock
            .Setup(uow => uow.Users.GetByIdAsync(userId))
            .ReturnsAsync((Core.Models.User)null);

        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _refreshUseCase.Execute(token));
        Assert.Equal($"User with id {userId} not found", exception.Message);

        _unitOfWorkMock.Verify(uow => uow.RefreshTokens.GetByTokenAsync(token), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Users.GetByIdAsync(userId), Times.Once);
        _jwtTokenServiceMock.Verify(jwt => jwt.GenerateToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
        _unitOfWorkMock.Verify(uow => uow.Complete(), Times.Never);
    }
}