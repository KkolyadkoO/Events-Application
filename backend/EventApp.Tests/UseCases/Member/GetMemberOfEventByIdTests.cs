using AutoMapper;
using EventApp.Application.DTOs.MemberOfEvent;
using EventApp.Application.UseCases.Member;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;
using EventApp.Core.Models;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.Member;

public class GetMemberOfEventByIdTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetMemberOfEventById _getMemberOfEventByIdUseCase;

    public GetMemberOfEventByIdTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _getMemberOfEventByIdUseCase = new GetMemberOfEventById(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenMemberNotFound()
    {
        var memberId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Members.GetById(memberId)).ReturnsAsync((MemberOfEvent)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _getMemberOfEventByIdUseCase.Execute(memberId));
    }

    [Fact]
    public async Task Execute_ShouldReturnMember_WhenMemberExists()
    {
        var memberId = Guid.NewGuid();
        var member = new MemberOfEvent();
        var responseDto = new MemberOfEventsResponseDto(memberId, "John", "Doe", DateTime.Now, "john@example.com", Guid.NewGuid(), Guid.NewGuid());

        _unitOfWorkMock.Setup(u => u.Members.GetById(memberId)).ReturnsAsync(member);
        _mapperMock.Setup(m => m.Map<MemberOfEventsResponseDto>(member)).Returns(responseDto);

        var result = await _getMemberOfEventByIdUseCase.Execute(memberId);

        Assert.Equal(responseDto, result);
    }
}