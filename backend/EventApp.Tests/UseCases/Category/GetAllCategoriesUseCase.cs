using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EventApp.Application.UseCases.Category;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Models;
using EventApp.Application.DTOs.CategoryOfEvent;

public class GetAllCategoriesUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllCategoriesUseCase _getAllCategoriesUseCase;

    public GetAllCategoriesUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _getAllCategoriesUseCase = new GetAllCategoriesUseCase(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldReturnListOfCategories_WhenCategoriesExist()
    {
        var categories = new List<CategoryOfEvent> { new CategoryOfEvent { Id = Guid.NewGuid(), Title = "Category 1" } };
        var responseDtos = new List<CategoryOfEventsResponseDto> { new CategoryOfEventsResponseDto(Guid.NewGuid(), "Category 1") };

        _unitOfWorkMock.Setup(u => u.Categories.Get())
            .ReturnsAsync(categories);
        _mapperMock.Setup(m => m.Map<List<CategoryOfEventsResponseDto>>(categories))
            .Returns(responseDtos);

        var result = await _getAllCategoriesUseCase.Execute();

        Assert.Equal(responseDtos, result);
    }
}