using AutoMapper;
using EventApp.Application.DTOs.MemberOfEvent;
using EventApp.Application.UseCases.Member;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Models;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.Member;

public class GetAllMembersOfEventByUserIdTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllMembersOfEventByUserId _getAllMembersOfEventByUserIdUseCase;

    public GetAllMembersOfEventByUserIdTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _getAllMembersOfEventByUserIdUseCase = new GetAllMembersOfEventByUserId(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldReturnListOfMembers_WhenMembersExist()
    {
        var userId = Guid.NewGuid();
        var members = new List<MemberOfEvent>
        {
            new MemberOfEvent(Guid.NewGuid(), "John", "Doe", DateTime.Now, DateTime.Now, "john@example.com", userId, Guid.NewGuid()),
            new MemberOfEvent(Guid.NewGuid(), "Jane", "Doe", DateTime.Now, DateTime.Now, "jane@example.com", userId, Guid.NewGuid())
        };
        var responseDtos = new List<MemberOfEventsResponseDto>
        {
            new MemberOfEventsResponseDto(Guid.NewGuid(), "John", "Doe", DateTime.Now, "john@example.com", userId, Guid.NewGuid()),
            new MemberOfEventsResponseDto(Guid.NewGuid(), "Jane", "Doe", DateTime.Now, "jane@example.com", userId, Guid.NewGuid())
        };

        _unitOfWorkMock.Setup(u => u.Members.GetByUserId(userId)).ReturnsAsync(members);
        _mapperMock.Setup(m => m.Map<List<MemberOfEventsResponseDto>>(members)).Returns(responseDtos);

        var result = await _getAllMembersOfEventByUserIdUseCase.Execute(userId);

        Assert.Equal(responseDtos.Count, result.Count);
        Assert.Equal(responseDtos[0].Name, result[0].Name);
    }
}