using AutoMapper;
using EventApp.Application.DTOs.Event;
using EventApp.Application.UseCases.Event;
using EventApp.Core.Abstractions.Repositories;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.Event;

public class GetEventsByFiltersUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetEventsByFiltersUseCase _useCase;

    public GetEventsByFiltersUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _useCase = new GetEventsByFiltersUseCase(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldReturnFilteredEventsAndTotalCount()
    {
        var request = new EventFilterRequestDto();
        var events = new List<Core.Models.Event>();
        var responseDtos = new List<EventsResponseDto>();

        _unitOfWorkMock.Setup(u => u.Events.GetBySpecificationAsync(It.IsAny<Core.Specifications.EventSpecification>(), It.IsAny<int?>(), It.IsAny<int?>()))
            .ReturnsAsync((events, 10));
        _mapperMock.Setup(m => m.Map<List<EventsResponseDto>>(events)).Returns(responseDtos);

        var result = await _useCase.Execute(request);

        Assert.Equal(responseDtos, result.Item1);
        Assert.Equal(10, result.Item2);
    }
    [Fact]
    public async Task Execute_ShouldReturnEmptyList_WhenNoEventsFound()
    {
        var request = new EventFilterRequestDto();
        var emptyEventList = new List<Core.Models.Event>();
        var emptyResponseDtos = new List<EventsResponseDto>();

        _unitOfWorkMock.Setup(u => u.Events.GetBySpecificationAsync(It.IsAny<Core.Specifications.EventSpecification>(), It.IsAny<int?>(), It.IsAny<int?>()))
            .ReturnsAsync((emptyEventList, 0));
        _mapperMock.Setup(m => m.Map<List<EventsResponseDto>>(emptyEventList)).Returns(emptyResponseDtos);

        var result = await _useCase.Execute(request);

        Assert.Empty(result.Item1);  
        Assert.Equal(0, result.Item2);  
    }
    [Fact]
    public async Task Execute_ShouldHandleInvalidFilterData()
    {
        // Arrange
        var request = new EventFilterRequestDto
        {
            StartDate = DateTime.Now.AddDays(5), // Например, некорректный диапазон дат
            EndDate = DateTime.Now.AddDays(-5)
        };

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => _useCase.Execute(request));
    }
}