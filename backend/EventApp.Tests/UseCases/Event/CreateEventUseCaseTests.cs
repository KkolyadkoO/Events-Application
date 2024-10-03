using AutoMapper;
using EventApp.Application.DTOs.Event;
using EventApp.Application.UseCases.Event;
using EventApp.Core.Abstractions.Repositories;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.Event;

public class CreateEventUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateEventUseCase _useCase;

    public CreateEventUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _useCase = new CreateEventUseCase(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldCreateEventAndReturnId()
    {
        var requestDto = new EventsRequestDto("Title", "Description", DateTime.Now, Guid.NewGuid(), 100, Guid.NewGuid());
        var imageFileMock = new Mock<IFormFile>();
        var newEvent = new EventApp.Core.Models.Event();

        _mapperMock.Setup(m => m.Map<EventApp.Core.Models.Event>(requestDto)).Returns(newEvent);
        _unitOfWorkMock.Setup(u => u.Events.Create(newEvent)).ReturnsAsync(Guid.NewGuid());

        var result = await _useCase.Execute(requestDto, imageFileMock.Object);

        _unitOfWorkMock.Verify(u => u.Events.Create(newEvent), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
        Assert.IsType<Guid>(result);
    }
}