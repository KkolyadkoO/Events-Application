using AutoMapper;
using EventApp.Application.DTOs.MemberOfEvent;
using EventApp.Application.UseCases.Member;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;
using EventApp.Core.Models;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.Member;

public class UpdateMemberOfEventTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateMemberOfEvent _updateMemberOfEventUseCase;

    public UpdateMemberOfEventTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _updateMemberOfEventUseCase = new UpdateMemberOfEvent(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenMemberNotFound()
    {
        var memberId = Guid.NewGuid();
        var requestDto = new MemberOfEventsRequestDto("John", "Doe", DateTime.Now, "john@example.com", Guid.NewGuid(), Guid.NewGuid());

        _unitOfWorkMock.Setup(u => u.Members.GetById(memberId)).ReturnsAsync((MemberOfEvent)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _updateMemberOfEventUseCase.Execute(memberId, requestDto));
    }

    [Fact]
    public async Task Execute_ShouldUpdateMember_WhenMemberExists()
    {
        var memberId = Guid.NewGuid();
        var member = new MemberOfEvent();
        var requestDto = new MemberOfEventsRequestDto("John", "Doe", DateTime.Now, "john@example.com", Guid.NewGuid(), Guid.NewGuid());

        _unitOfWorkMock.Setup(u => u.Members.GetById(memberId)).ReturnsAsync(member);
        _mapperMock.Setup(m => m.Map(requestDto, member)).Returns(member);
        _unitOfWorkMock.Setup(u => u.Members.Update(member)).ReturnsAsync(true);

        await _updateMemberOfEventUseCase.Execute(memberId, requestDto);

        _unitOfWorkMock.Verify(u => u.Members.Update(member), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
    }
}