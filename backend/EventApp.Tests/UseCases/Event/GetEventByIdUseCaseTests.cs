using AutoMapper;
using EventApp.Application.DTOs.Event;
using EventApp.Application.UseCases.Event;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.Event;

public class GetEventByIdUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetEventByIdUseCase _useCase;

    public GetEventByIdUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _useCase = new GetEventByIdUseCase(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldReturnEvent_WhenEventExists()
    {
        var eventId = Guid.NewGuid();
        var existingEvent = new Core.Models.Event();
        var responseDto = new EventsResponseDto();

        _unitOfWorkMock.Setup(u => u.Events.GetById(eventId)).ReturnsAsync(existingEvent);
        _mapperMock.Setup(m => m.Map<EventsResponseDto>(existingEvent)).Returns(responseDto);

        var result = await _useCase.Execute(eventId);

        Assert.Equal(responseDto, result);
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenEventDoesNotExist()
    {
        var eventId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Events.GetById(eventId)).ReturnsAsync((Core.Models.Event)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _useCase.Execute(eventId));
    }
}