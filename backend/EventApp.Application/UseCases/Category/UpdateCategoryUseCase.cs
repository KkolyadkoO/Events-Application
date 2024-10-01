using AutoMapper;
using EventApp.Application.DTOs.CategoryOfEvent;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;

namespace EventApp.Application.UseCases.Category;

public class UpdateCategoryUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCategoryUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Execute(Guid id, CategoryOfEventsRequestDto requestDto)
    {
        var category = await _unitOfWork.Categories.GetById(id);
        
        if (category == null)
        {
            throw new NotFoundException($"Category with ID '{id}' not found.");
        }
        category.Title = requestDto.Title;
        
        await _unitOfWork.Categories.Update(category);
        await _unitOfWork.Complete();

        return id;
    }
}