using EventApp.Application;
using EventApp.Core.Abstractions;
using EventApp.Core.Exceptions;
using EventApp.Core.Models;
using EventApp.Infrastructure;
using Moq;
using Xunit;

namespace EventApp.Tests.Services;

public class RefreshTokenServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
        private readonly RefreshTokenService _refreshTokenService;

        public RefreshTokenServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _jwtTokenServiceMock = new Mock<IJwtTokenService>();
            _refreshTokenService = new RefreshTokenService(_unitOfWorkMock.Object, _jwtTokenServiceMock.Object);
        }

        [Fact]
        public async Task RefreshToken_ReturnsNewTokens_WhenRefreshTokenIsValid()
        {
            var refreshToken = "validToken";
            var userId = Guid.NewGuid();
            var storedRefreshToken = new RefreshToken(Guid.NewGuid(), userId, refreshToken, DateTime.Now.AddMinutes(5));
            var generatedTokens = ("newAccessToken", "newRefreshToken");

            _unitOfWorkMock.Setup(u => u.RefreshTokens.Get(refreshToken))
                .ReturnsAsync(storedRefreshToken);

            var user = new User(userId, "testUser", "test@example.com", "password", "user");

            _unitOfWorkMock.Setup(u => u.Users.GetById(userId))
                .ReturnsAsync(user);

            _jwtTokenServiceMock.Setup(j => j.GenerateToken(userId, user.UserName, user.Role))
                .ReturnsAsync(generatedTokens);

            var result = await _refreshTokenService.RefreshToken(refreshToken);

            Assert.Equal(generatedTokens, result);
            _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
        }

        [Fact]
        public async Task RefreshToken_ThrowsInvalidRefreshToken_WhenTokenIsExpired()
        {
            var refreshToken = "expiredToken";
            var expiredRefreshToken = new RefreshToken(Guid.NewGuid(), Guid.NewGuid(), refreshToken, DateTime.Now.AddMinutes(-5));

            _unitOfWorkMock.Setup(u => u.RefreshTokens.Get(refreshToken))
                .ReturnsAsync(expiredRefreshToken);

            var exception = await Assert.ThrowsAsync<RefreshTokenNotFound>(() => _refreshTokenService.RefreshToken(refreshToken));
            Assert.Equal("Invalid or expired refresh token", exception.Message);
        }

        [Fact]
        public async Task RefreshToken_ThrowsRefreshTokenNotFound_WhenTokenDoesNotExist()
        {
            var refreshToken = "nonExistentToken";

            _unitOfWorkMock.Setup(u => u.RefreshTokens.Get(refreshToken))
                .ThrowsAsync(new Exception("Token not found"));

            var exception = await Assert.ThrowsAsync<RefreshTokenNotFound>(() => _refreshTokenService.RefreshToken(refreshToken));
            Assert.Equal("Token not found", exception.Message);
        }

        [Fact]
        public async Task DeleteRefreshToken_ReturnsDeletedToken_WhenSuccessful()
        {
            var refreshToken = "validToken";

            _unitOfWorkMock.Setup(u => u.RefreshTokens.Delete(refreshToken))
                .ReturnsAsync(refreshToken);

            var result = await _refreshTokenService.DeleteRefreshToken(refreshToken);

            Assert.Equal(refreshToken, result);
        }

        [Fact]
        public async Task DeleteRefreshToken_ThrowsRefreshTokenNotFound_WhenTokenDoesNotExist()
        {
            var refreshToken = "nonExistentToken";

            _unitOfWorkMock.Setup(u => u.RefreshTokens.Delete(refreshToken))
                .ThrowsAsync(new Exception("Token not found"));

            var exception = await Assert.ThrowsAsync<RefreshTokenNotFound>(() => _refreshTokenService.DeleteRefreshToken(refreshToken));
            Assert.Equal("Token not found", exception.Message);
        }

        [Fact]
        public async Task GetRefreshToken_ReturnsToken_WhenSuccessful()
        {
            var refreshToken = "validToken";
            var storedRefreshToken = new RefreshToken(Guid.NewGuid(), Guid.NewGuid(), refreshToken, DateTime.Now.AddMinutes(5));

            _unitOfWorkMock.Setup(u => u.RefreshTokens.Get(refreshToken))
                .ReturnsAsync(storedRefreshToken);

            var result = await _refreshTokenService.GetRefreshToken(refreshToken);

            Assert.Equal(storedRefreshToken, result);
        }

        [Fact]
        public async Task GetRefreshToken_ThrowsRefreshTokenNotFound_WhenTokenDoesNotExist()
        {
            var refreshToken = "nonExistentToken";

            _unitOfWorkMock.Setup(u => u.RefreshTokens.Get(refreshToken))
                .ThrowsAsync(new Exception("Token not found"));

            var exception = await Assert.ThrowsAsync<RefreshTokenNotFound>(() => _refreshTokenService.GetRefreshToken(refreshToken));
            Assert.Equal("Token not found", exception.Message);
        }
    }