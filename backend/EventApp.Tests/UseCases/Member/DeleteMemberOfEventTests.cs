using EventApp.Application.UseCases.Member;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;
using EventApp.Core.Models;
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
        // Arrange
        var memberId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Members.GetById(memberId)).ReturnsAsync((MemberOfEvent)null);

        var useCase = new DeleteMemberOfEvent(_unitOfWorkMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => useCase.Execute(memberId));
        _unitOfWorkMock.Verify(u => u.Members.GetById(memberId), Times.Once);
    }

    [Fact]
    public async Task DeleteMemberOfEvent_Should_Delete_When_Found()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        var member = new MemberOfEvent(memberId, "John", "Doe", DateTime.Today, DateTime.Now, "john@example.com", Guid.NewGuid(), Guid.NewGuid());
        _unitOfWorkMock.Setup(u => u.Members.GetById(memberId)).ReturnsAsync(member);
        _unitOfWorkMock.Setup(u => u.Members.Delete(memberId)).Returns(Task.CompletedTask);

        var useCase = new DeleteMemberOfEvent(_unitOfWorkMock.Object);

        // Act
        await useCase.Execute(memberId);

        // Assert
        _unitOfWorkMock.Verify(u => u.Members.Delete(memberId), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
    }
}