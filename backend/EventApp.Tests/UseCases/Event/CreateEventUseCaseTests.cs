using AutoMapper;
using EventApp.Application.DTOs.Event;
using EventApp.Application.UseCases.Event;
using EventApp.Core.Abstractions;
using EventApp.Core.Abstractions.Repositories;
using EventApp.DataAccess.Abstractions;
using EventApp.Infrastructure;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.Event;

public class CreateEventUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IImageService> _imageServiceMock;
    private readonly CreateEventUseCase _useCase;

    public CreateEventUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _imageServiceMock = new Mock<IImageService>();
        _useCase = new CreateEventUseCase(_unitOfWorkMock.Object, _mapperMock.Object, _imageServiceMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldCreateEventAndReturnId()
    {
        var requestDto = new EventsRequestDto("Title", "Description", DateTime.Now, Guid.NewGuid(), 100, Guid.NewGuid());
        var imageFileMock = new Mock<IFormFile>();
        imageFileMock.Setup(f => f.Length).Returns(1);
        imageFileMock.Setup(f => f.FileName).Returns("new-image.jpg");
        imageFileMock.Setup(f => f.ContentType).Returns("image/jpeg");
        
        var newEvent = new EventApp.Core.Models.Event();

        _mapperMock.Setup(m => m.Map<EventApp.Core.Models.Event>(requestDto)).Returns(newEvent);
        _unitOfWorkMock.Setup(u => u.Events.AddAsync(newEvent)).ReturnsAsync(Guid.NewGuid());
        

        var result = await _useCase.Execute(requestDto, imageFileMock.Object);

        _unitOfWorkMock.Verify(u => u.Events.AddAsync(newEvent), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
        Assert.IsType<Guid>(result);
    }
}