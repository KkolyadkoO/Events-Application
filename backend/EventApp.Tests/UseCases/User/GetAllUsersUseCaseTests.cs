using AutoMapper;
using EventApp.Application.DTOs.User;
using EventApp.Application.UseCases.User;
using EventApp.Core.Abstractions.Repositories;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.User;

public class GetAllUsersUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllUsersUseCase _getAllUsersUseCase;

    public GetAllUsersUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _getAllUsersUseCase = new GetAllUsersUseCase(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldReturnMappedUsers_WhenUsersExist()
    {
        var users = new List<Core.Models.User> { new Core.Models.User() };
        _unitOfWorkMock.Setup(u => u.Users.Get()).ReturnsAsync(users);

        var mappedUsers = new List<UsersResponseDto> { new UsersResponseDto(Guid.NewGuid(), "TestUser", "test@example.com", "user") };
        _mapperMock.Setup(m => m.Map<List<UsersResponseDto>>(users)).Returns(mappedUsers);

        var result = await _getAllUsersUseCase.Execute();

        Assert.Equal(mappedUsers.Count, result.Count);
        _unitOfWorkMock.Verify(u => u.Users.Get(), Times.Once);
        _mapperMock.Verify(m => m.Map<List<UsersResponseDto>>(users), Times.Once);
    }
}