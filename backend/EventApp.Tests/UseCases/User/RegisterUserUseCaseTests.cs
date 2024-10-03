using AutoMapper;
using EventApp.Application.DTOs.User;
using EventApp.Application.UseCases.User;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;
using EventApp.Infrastructure;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.User;

public class RegisterUserUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly RegisterUserUseCase _registerUserUseCase;

    public RegisterUserUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _registerUserUseCase = new RegisterUserUseCase(_unitOfWorkMock.Object, _passwordHasherMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldThrowDuplicateUsersException_WhenUserWithEmailExists()
    {
        // Arrange
        var registerRequest = new UserRegisterRequestDto("username", "test@example.com", "password", "user");
        var existingUser = new Core.Models.User(Guid.NewGuid(), "existingUser", "test@example.com", "password", "user");
        _unitOfWorkMock.Setup(u => u.Users.GetByEmail(registerRequest.UserEmail)).ReturnsAsync(existingUser);

        // Act & Assert
        await Assert.ThrowsAsync<DuplicateUsers>(() => _registerUserUseCase.Execute(registerRequest));
    }

    [Fact]
    public async Task Execute_ShouldCreateUser_WhenRegistrationIsSuccessful()
    {
        var registerRequest = new UserRegisterRequestDto("username", "test@example.com", "password", "user");
        _unitOfWorkMock.Setup(u => u.Users.GetByEmail(registerRequest.UserEmail)).ReturnsAsync((Core.Models.User)null); 

        var newUser = new Core.Models.User(Guid.NewGuid(), registerRequest.Username, registerRequest.UserEmail, "hashedPassword", registerRequest.Role);
        _mapperMock.Setup(m => m.Map<Core.Models.User>(registerRequest)).Returns(newUser);
        _passwordHasherMock.Setup(p => p.HashPassword(registerRequest.Password)).Returns("hashedPassword");

        await _registerUserUseCase.Execute(registerRequest);

        _unitOfWorkMock.Verify(u => u.Users.Create(It.IsAny<Core.Models.User>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
        _passwordHasherMock.Verify(p => p.HashPassword(registerRequest.Password), Times.Once);
        _mapperMock.Verify(m => m.Map<Core.Models.User>(registerRequest), Times.Once);
    }
}