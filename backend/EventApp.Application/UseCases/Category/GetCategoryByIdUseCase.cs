using AutoMapper;
using EventApp.Application.DTOs.CategoryOfEvent;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;

namespace EventApp.Application.UseCases.Category;

public class GetCategoryByIdUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoryByIdUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoryOfEventsResponseDto> Execute(Guid id)
    {
        var category = await _unitOfWork.Categories.GetById(id);
        if (category == null)
        {
            throw new NotFoundException($"Category with ID {id} not found.");
        }
        return _mapper.Map<CategoryOfEventsResponseDto>(category);
    }
}