using EventApp.Application.UseCases.Member;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;
using EventApp.Core.Models;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.Member;

public class DeleteMemberOfEventByEventIdAndUserIdTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteMemberOfEventByEventIdAndUserId _deleteMemberOfEventByEventIdAndUserId;

    public DeleteMemberOfEventByEventIdAndUserIdTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _deleteMemberOfEventByEventIdAndUserId = new DeleteMemberOfEventByEventIdAndUserId(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task DeleteMemberOfEventByEventIdAndUserId_Should_Throw_NotFoundException_When_Member_Not_Found()
    {
        var eventId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Members.GetByEventIdAndUserId(eventId, userId)).ReturnsAsync((MemberOfEvent)null);


        await Assert.ThrowsAsync<NotFoundException>(() =>
            _deleteMemberOfEventByEventIdAndUserId.Execute(eventId, userId));
        _unitOfWorkMock.Verify(u => u.Members.GetByEventIdAndUserId(eventId, userId), Times.Once);
    }

    [Fact]
    public async Task DeleteMemberOfEventByEventIdAndUserId_Should_Delete_When_Found()
    {
        var eventId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var member = new MemberOfEvent(Guid.NewGuid(), "John", "Doe", DateTime.Today, DateTime.Now, "john@example.com",
            userId, eventId);
        _unitOfWorkMock.Setup(u => u.Members.GetByEventIdAndUserId(eventId, userId)).ReturnsAsync(member);
        _unitOfWorkMock.Setup(u => u.Members.DeleteByEventIdAndUserId(eventId, userId)).ReturnsAsync(true);


        await _deleteMemberOfEventByEventIdAndUserId.Execute(eventId, userId);

        _unitOfWorkMock.Verify(u => u.Members.DeleteByEventIdAndUserId(eventId, userId), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
    }
}