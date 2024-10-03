using AutoMapper;
using EventApp.Application.DTOs.MemberOfEvent;
using EventApp.Application.UseCases.Member;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Models;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.Member;

public class GetAllMembersOfEventByEventIdTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllMembersOfEventByEventId _getAllMembersOfEventByEventIdUseCase;
    
    public GetAllMembersOfEventByEventIdTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _getAllMembersOfEventByEventIdUseCase = new GetAllMembersOfEventByEventId(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllMembersOfEventByEventId_Should_Return_Members()
    {
        var eventId = Guid.NewGuid();
        var members = new List<MemberOfEvent> 
        { 
            new MemberOfEvent(Guid.NewGuid(), "John", "Doe", DateTime.Today, DateTime.Now, "john@example.com", Guid.NewGuid(), eventId),
            new MemberOfEvent(Guid.NewGuid(), "Jane", "Doe", DateTime.Today, DateTime.Now, "jane@example.com", Guid.NewGuid(), eventId)
        };
        var responseDtos = new List<MemberOfEventsResponseDto>
        {
            new MemberOfEventsResponseDto(Guid.NewGuid(), "John", "Doe", DateTime.Today, "john@example.com", Guid.NewGuid(), eventId),
            new MemberOfEventsResponseDto(Guid.NewGuid(), "Jane", "Doe", DateTime.Today, "jane@example.com", Guid.NewGuid(), eventId)
        };

        _unitOfWorkMock.Setup(u => u.Members.GetByEventId(eventId)).ReturnsAsync(members);
        _mapperMock.Setup(m => m.Map<List<MemberOfEventsResponseDto>>(members)).Returns(responseDtos);


        var result = await _getAllMembersOfEventByEventIdUseCase.Execute(eventId);

        Assert.Equal(responseDtos, result);
        _unitOfWorkMock.Verify(u => u.Members.GetByEventId(eventId), Times.Once);
    }
}