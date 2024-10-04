using AutoMapper;
using EventApp.Application.DTOs.CategoryOfEvent;
using EventApp.Application.Exceptions;
using EventApp.Core.Models;
using EventApp.DataAccess.Abstractions;

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
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        
        if (category == null)
        {
            throw new NotFoundException($"Category with ID '{id}' not found.");
        }
        var updateCategory = _mapper.Map<CategoryOfEvent>(requestDto);
        updateCategory.Id = id;
        
        await _unitOfWork.Categories.UpdateAsync(updateCategory);
        await _unitOfWork.Complete();

        return id;
    }
}