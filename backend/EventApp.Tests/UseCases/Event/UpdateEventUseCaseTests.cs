using AutoMapper;
using EventApp.Application.DTOs.Event;
using EventApp.Application.UseCases.Event;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.Event;

public class UpdateEventUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateEventUseCase _useCase;

    public UpdateEventUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _useCase = new UpdateEventUseCase(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldUpdateEvent_WhenEventExists()
    {
        var eventId = Guid.NewGuid();
        var existingEvent = new EventApp.Core.Models.Event { ImageUrl = "old-image.jpg" };
        var requestDto = new EventsRequestDto("Updated Title", "Updated Description", DateTime.Now, Guid.NewGuid(), 100, Guid.NewGuid());
        var imageFileMock = new Mock<IFormFile>();

        _unitOfWorkMock.Setup(u => u.Events.GetById(eventId)).ReturnsAsync(existingEvent);

        await _useCase.Execute(eventId, requestDto, null);

        _unitOfWorkMock.Verify(u => u.Events.Update(existingEvent), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenEventDoesNotExist()
    {
        var eventId = Guid.NewGuid();
        var requestDto = new EventsRequestDto("Title", "Description", DateTime.Now, Guid.NewGuid(), 100, Guid.NewGuid());

        _unitOfWorkMock.Setup(u => u.Events.GetById(eventId)).ReturnsAsync((EventApp.Core.Models.Event)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _useCase.Execute(eventId, requestDto, null));
    }

    [Fact]
    public async Task Execute_ShouldUpdateImage_WhenImageProvided()
    {
        var eventId = Guid.NewGuid();
        var existingEvent = new EventApp.Core.Models.Event { ImageUrl = "old-image.jpg" };
        var requestDto = new EventsRequestDto("Title", "Description", DateTime.Now, Guid.NewGuid(), 100, Guid.NewGuid());
        var imageFileMock = new Mock<IFormFile>();

        imageFileMock.Setup(f => f.Length).Returns(1);
        imageFileMock.Setup(f => f.FileName).Returns("new-image.jpg");
        imageFileMock.Setup(f => f.ContentType).Returns("image/jpeg");

        _unitOfWorkMock.Setup(u => u.Events.GetById(eventId)).ReturnsAsync(existingEvent);

        await _useCase.Execute(eventId, requestDto, imageFileMock.Object);

        _unitOfWorkMock.Verify(u => u.Events.Update(existingEvent), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
    }
}