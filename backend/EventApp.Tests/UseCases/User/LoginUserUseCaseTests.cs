using AutoMapper;
using EventApp.Application.DTOs.User;
using EventApp.Application.Exceptions;
using EventApp.Application.UseCases.User;
using EventApp.Core.Abstractions;
using EventApp.Core.Abstractions.Repositories;
using EventApp.DataAccess.Abstractions;
using EventApp.Infrastructure;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.User;

public class LoginUserUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly LoginUserUseCase _loginUserUseCase;

    public LoginUserUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _loginUserUseCase = new LoginUserUseCase(_unitOfWorkMock.Object, _passwordHasherMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldThrowInvalidLoginException_WhenUserDoesNotExist()
    {
        var loginRequest = new UserLoginRequestDto("username", "password");
        _unitOfWorkMock.Setup(u => u.Users.GetByLoginAsync(loginRequest.UserName)).ReturnsAsync((Core.Models.User)null);

        await Assert.ThrowsAsync<InvalidLoginException>(() => _loginUserUseCase.Execute(loginRequest));
    }

    [Fact]
    public async Task Execute_ShouldThrowInvalidLoginException_WhenPasswordIsIncorrect()
    {
        var loginRequest = new UserLoginRequestDto("username", "password");
        var user = new Core.Models.User(Guid.NewGuid(), "username", "test@example.com", "hashedPassword", "user");
        _unitOfWorkMock.Setup(u => u.Users.GetByLoginAsync(loginRequest.UserName)).ReturnsAsync(user);
        _passwordHasherMock.Setup(p => p.VerifyHashedPassword(user.Password, loginRequest.Password)).Returns(false);

        await Assert.ThrowsAsync<InvalidLoginException>(() => _loginUserUseCase.Execute(loginRequest));
    }

    [Fact]
    public async Task Execute_ShouldReturnMappedUser_WhenLoginIsSuccessful()
    {
        var loginRequest = new UserLoginRequestDto("username", "password");
        var user = new Core.Models.User(Guid.NewGuid(), "username", "test@example.com", "hashedPassword", "user");
        _unitOfWorkMock.Setup(u => u.Users.GetByLoginAsync(loginRequest.UserName)).ReturnsAsync(user);
        _passwordHasherMock.Setup(p => p.VerifyHashedPassword(user.Password, loginRequest.Password)).Returns(true);

        var mappedUser = new UsersResponseDto(user.Id, "username", "test@example.com", "user");
        _mapperMock.Setup(m => m.Map<UsersResponseDto>(user)).Returns(mappedUser);

        var result = await _loginUserUseCase.Execute(loginRequest);

        Assert.Equal(mappedUser.Id, result.Id);
        _unitOfWorkMock.Verify(u => u.Users.GetByLoginAsync(loginRequest.UserName), Times.Once);
        _passwordHasherMock.Verify(p => p.VerifyHashedPassword(user.Password, loginRequest.Password), Times.Once);
        _mapperMock.Verify(m => m.Map<UsersResponseDto>(user), Times.Once);
    }
}