using AutoMapper;
using EventApp.Application.DTOs.MemberOfEvent;
using EventApp.Application.UseCases.Member;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Models;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.Member;

public class AddMemberOfEventTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AddMemberOfEvent _addMemberOfEvent;

    public AddMemberOfEventTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _addMemberOfEvent = new AddMemberOfEvent(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task AddMemberOfEvent_Should_Return_Id_When_Successful()
    {
        var memberId = Guid.NewGuid();
        var requestDto = new MemberOfEventsRequestDto("John", "Doe", DateTime.Today, "john@example.com", Guid.NewGuid(),
            Guid.NewGuid());
        var memberOfEvent = new MemberOfEvent(memberId, "John", "Doe", DateTime.Today, DateTime.Now, "john@example.com",
            Guid.NewGuid(), Guid.NewGuid());

        _mapperMock.Setup(m => m.Map<MemberOfEvent>(requestDto)).Returns(memberOfEvent);
        _unitOfWorkMock.Setup(u => u.Members.Create(It.IsAny<MemberOfEvent>())).ReturnsAsync(memberId);


        var result = await _addMemberOfEvent.Execute(requestDto);

        Assert.Equal(memberId, result);
        _unitOfWorkMock.Verify(u => u.Members.Create(memberOfEvent), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
    }
}