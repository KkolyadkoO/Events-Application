using AutoMapper;
using EventApp.Application.DTOs.Event;
using EventApp.Application.Exceptions;
using EventApp.Application.UseCases.Event;
using EventApp.DataAccess.Abstractions;
using EventApp.Infrastructure;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.Event;

public class UpdateEventUseCaseTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IImageService> _mockImageService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UpdateEventUseCase _updateEventUseCase;

    public UpdateEventUseCaseTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockImageService = new Mock<IImageService>();
        _mockMapper = new Mock<IMapper>();
        _updateEventUseCase = new UpdateEventUseCase(_mockUnitOfWork.Object, _mockImageService.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Execute_EventExists_UpdatesEventWithoutImage()
    {
        var eventId = Guid.NewGuid();
        var requestDto = new EventsRequestDto("Updated Event", "Updated Description", DateTime.Now, Guid.NewGuid(), 100, Guid.NewGuid());
        var existingEvent = new Core.Models.Event(eventId, "Original Event", "Original Description", DateTime.Now.AddDays(-1), Guid.NewGuid(), Guid.NewGuid(), 50, null);
        var updatedEvent = new Core.Models.Event(eventId, requestDto.Title, requestDto.Description, requestDto.Date, requestDto.LocationId, requestDto.CategoryId, requestDto.MaxNumberOfMembers, null);

        _mockUnitOfWork.Setup(u => u.Events.GetByIdAsync(eventId)).ReturnsAsync(existingEvent);
        _mockMapper.Setup(m => m.Map<Core.Models.Event>(requestDto)).Returns(updatedEvent);

        await _updateEventUseCase.Execute(eventId, requestDto, null);

        _mockUnitOfWork.Verify(u => u.Events.UpdateAsync(updatedEvent), Times.Once);
        _mockUnitOfWork.Verify(u => u.Complete(), Times.Once);
        _mockImageService.Verify(i => i.UpdateImageToFileSystem(It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Execute_EventExists_UpdatesEventWithImage()
    {
        var eventId = Guid.NewGuid();
        var requestDto = new EventsRequestDto("Updated Event", "Updated Description", DateTime.Now, Guid.NewGuid(), 100, Guid.NewGuid());
        var existingEvent = new Core.Models.Event(eventId, "Original Event", "Original Description", DateTime.Now.AddDays(-1), Guid.NewGuid(), Guid.NewGuid(), 50, "oldImage.png");
        var updatedEvent = new Core.Models.Event(eventId, requestDto.Title, requestDto.Description, requestDto.Date, requestDto.LocationId, requestDto.CategoryId, requestDto.MaxNumberOfMembers, "newImage.png");

        var mockImageFile = new Mock<IFormFile>();
        var fileName = "newImage.png";
        var memoryStream = new MemoryStream();
        var writer = new StreamWriter(memoryStream);
        writer.Write("This is a test file");
        writer.Flush();
        memoryStream.Position = 0;

        mockImageFile.Setup(f => f.FileName).Returns(fileName);
        mockImageFile.Setup(f => f.ContentType).Returns("image/png");
        mockImageFile.Setup(f => f.OpenReadStream()).Returns(memoryStream);

        _mockUnitOfWork.Setup(u => u.Events.GetByIdAsync(eventId)).ReturnsAsync(existingEvent);
        _mockMapper.Setup(m => m.Map<Core.Models.Event>(requestDto)).Returns(updatedEvent);
        _mockImageService.Setup(i => i.UpdateImageToFileSystem(mockImageFile.Object, existingEvent.ImageUrl)).ReturnsAsync("newImage.png");

        await _updateEventUseCase.Execute(eventId, requestDto, mockImageFile.Object);

        _mockUnitOfWork.Verify(u => u.Events.UpdateAsync(updatedEvent), Times.Once);
        _mockUnitOfWork.Verify(u => u.Complete(), Times.Once);
        _mockImageService.Verify(i => i.UpdateImageToFileSystem(mockImageFile.Object, existingEvent.ImageUrl), Times.Once);
    }

    [Fact]
    public async Task Execute_EventNotFound_ThrowsNotFoundException()
    {
        var eventId = Guid.NewGuid();
        var requestDto = new EventsRequestDto("Updated Event", "Updated Description", DateTime.Now, Guid.NewGuid(), 100, Guid.NewGuid());

        _mockUnitOfWork.Setup(u => u.Events.GetByIdAsync(eventId)).ReturnsAsync((Core.Models.Event)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _updateEventUseCase.Execute(eventId, requestDto, null));
        _mockUnitOfWork.Verify(u => u.Events.UpdateAsync(It.IsAny<Core.Models.Event>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.Complete(), Times.Never);
    }

    [Fact]
    public async Task Execute_InvalidImageFile_ThrowsInvalidOperationException()
    {
        var eventId = Guid.NewGuid();
        var requestDto = new EventsRequestDto("Updated Event", "Updated Description", DateTime.Now, Guid.NewGuid(), 100, Guid.NewGuid());
        var existingEvent = new Core.Models.Event(eventId, "Original Event", "Original Description", DateTime.Now.AddDays(-1), Guid.NewGuid(), Guid.NewGuid(), 50, "oldImage.png");

        var mockImageFile = new Mock<IFormFile>();
        mockImageFile.Setup(f => f.FileName).Returns("invalidImage.txt");
        mockImageFile.Setup(f => f.ContentType).Returns("text/plain");

        _mockUnitOfWork.Setup(u => u.Events.GetByIdAsync(eventId)).ReturnsAsync(existingEvent);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _updateEventUseCase.Execute(eventId, requestDto, mockImageFile.Object));
        _mockUnitOfWork.Verify(u => u.Events.UpdateAsync(It.IsAny<Core.Models.Event>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.Complete(), Times.Never);
        _mockImageService.Verify(i => i.UpdateImageToFileSystem(It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Never);
    }
}