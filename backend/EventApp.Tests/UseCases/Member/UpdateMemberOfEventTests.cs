using AutoMapper;
using EventApp.Application.DTOs.MemberOfEvent;
using EventApp.Application.Exceptions;
using EventApp.Application.UseCases.Member;
using EventApp.Core.Models;
using EventApp.DataAccess.Abstractions;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.Member;

public class UpdateMemberOfEventTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UpdateMemberOfEvent _updateMemberOfEventUseCase;

    public UpdateMemberOfEventTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _updateMemberOfEventUseCase = new UpdateMemberOfEvent(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Execute_MemberOfEventExists_UpdatesMemberAndCompletes()
    {
        var memberId = Guid.NewGuid();
        var requestDto = new MemberOfEventsRequestDto("John", "Doe", new DateTime(1990, 1, 1), "john@example.com", Guid.NewGuid(), Guid.NewGuid());
        var existingMember = new MemberOfEvent(memberId, "Jane", "Doe", new DateTime(1985, 1, 1), DateTime.Now, "jane@example.com", Guid.NewGuid(), Guid.NewGuid());
        var updatedMember = new MemberOfEvent(memberId, "John", "Doe", new DateTime(1990, 1, 1), DateTime.Now, "john@example.com", Guid.NewGuid(), Guid.NewGuid());

        _mockUnitOfWork.Setup(u => u.Members.GetByIdAsync(memberId))
            .ReturnsAsync(existingMember);

        _mockMapper.Setup(m => m.Map<MemberOfEvent>(requestDto))
            .Returns(updatedMember);

        await _updateMemberOfEventUseCase.Execute(memberId, requestDto);

        _mockUnitOfWork.Verify(u => u.Members.UpdateAsync(updatedMember), Times.Once);
        _mockUnitOfWork.Verify(u => u.Complete(), Times.Once);
    }

    [Fact]
    public async Task Execute_MemberOfEventNotFound_ThrowsNotFoundException()
    {
        var memberId = Guid.NewGuid();
        var requestDto = new MemberOfEventsRequestDto("John", "Doe", new DateTime(1990, 1, 1), "john@example.com", Guid.NewGuid(), Guid.NewGuid());

        _mockUnitOfWork.Setup(u => u.Members.GetByIdAsync(memberId))
            .ReturnsAsync((MemberOfEvent)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _updateMemberOfEventUseCase.Execute(memberId, requestDto));
        _mockUnitOfWork.Verify(u => u.Members.UpdateAsync(It.IsAny<MemberOfEvent>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.Complete(), Times.Never);
    }
}