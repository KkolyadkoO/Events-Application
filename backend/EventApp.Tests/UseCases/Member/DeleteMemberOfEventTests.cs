using EventApp.Application.Exceptions;
using EventApp.Application.UseCases.Member;
using EventApp.Core.Abstractions;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Models;
using EventApp.DataAccess.Abstractions;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.Member;

public class DeleteMemberOfEventTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public DeleteMemberOfEventTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task DeleteMemberOfEvent_Should_Throw_NotFoundException_When_Member_Not_Found()
    {
        var memberId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Members.GetByIdAsync(memberId)).ReturnsAsync((MemberOfEvent)null);

        var useCase = new DeleteMemberOfEvent(_unitOfWorkMock.Object);

        await Assert.ThrowsAsync<NotFoundException>(() => useCase.Execute(memberId));
        _unitOfWorkMock.Verify(u => u.Members.GetByIdAsync(memberId), Times.Once);
    }

    [Fact]
    public async Task DeleteMemberOfEvent_Should_Delete_When_Found()
    {
        var memberId = Guid.NewGuid();
        var member = new MemberOfEvent(memberId, "John", "Doe", DateTime.Today, DateTime.Now, "john@example.com",
            Guid.NewGuid(), Guid.NewGuid());
        _unitOfWorkMock.Setup(u => u.Members.GetByIdAsync(memberId)).ReturnsAsync(member);
        _unitOfWorkMock.Setup(u => u.Members.DeleteAsync(memberId)).Returns(Task.CompletedTask);

        var useCase = new DeleteMemberOfEvent(_unitOfWorkMock.Object);

        await useCase.Execute(memberId);
        
        _unitOfWorkMock.Verify(u => u.Members.DeleteAsync(memberId), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
    }
}