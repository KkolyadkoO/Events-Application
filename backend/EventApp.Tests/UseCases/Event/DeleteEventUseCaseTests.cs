using EventApp.Application.Exceptions;
using EventApp.Application.UseCases.Event;
using EventApp.Core.Abstractions;
using EventApp.Core.Abstractions.Repositories;
using EventApp.DataAccess.Abstractions;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.Event;

public class DeleteEventUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteEventUseCase _useCase;

    public DeleteEventUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _useCase = new DeleteEventUseCase(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldDeleteEvent_WhenEventExists()
    {
        var eventId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Events.GetByIdAsync(eventId)).ReturnsAsync(new EventApp.Core.Models.Event());

        await _useCase.Execute(eventId);

        _unitOfWorkMock.Verify(u => u.Events.DeleteAsync(eventId), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenEventDoesNotExist()
    {
        var eventId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Events.GetByIdAsync(eventId)).ReturnsAsync((EventApp.Core.Models.Event)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _useCase.Execute(eventId));
    }
}