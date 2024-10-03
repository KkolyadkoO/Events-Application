using AutoMapper;
using EventApp.Application.DTOs.User;
using EventApp.Application.UseCases.User;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.User;

public class GetUserByIdUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetUserByIdUseCase _getUserByIdUseCase;

    public GetUserByIdUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _getUserByIdUseCase = new GetUserByIdUseCase(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
    {
        var userId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Users.GetById(userId)).ReturnsAsync((Core.Models.User)null);

        await Assert.ThrowsAsync<UserNotFound>(() => _getUserByIdUseCase.Execute(userId));
    }

    [Fact]
    public async Task Execute_ShouldReturnMappedUser_WhenUserExists()
    {
        var userId = Guid.NewGuid();
        var user = new Core.Models.User(userId, "TestUser", "test@example.com", "password", "user");
        _unitOfWorkMock.Setup(u => u.Users.GetById(userId)).ReturnsAsync(user);

        var mappedUser = new UsersResponseDto(userId, "TestUser", "test@example.com", "user");
        _mapperMock.Setup(m => m.Map<UsersResponseDto>(user)).Returns(mappedUser);

        var result = await _getUserByIdUseCase.Execute(userId);

        Assert.Equal(mappedUser.Id, result.Id);
        _unitOfWorkMock.Verify(u => u.Users.GetById(userId), Times.Once);
        _mapperMock.Verify(m => m.Map<UsersResponseDto>(user), Times.Once);
    }
}