using AutoMapper;
using EventApp.Application.DTOs.CategoryOfEvent;
using EventApp.Application.Exceptions;
using EventApp.Core.Models;
using EventApp.DataAccess.Abstractions;

namespace EventApp.Application.UseCases.Category;

public class AddCategoryUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddCategoryUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Execute(CategoryOfEventsRequestDto requestDto)
    {
        var existingCategory = await _unitOfWork.Categories.GetByTitleAsync(requestDto.Title);
        if (existingCategory != null)
        {
            throw new DuplicateCategory($"Category with title '{requestDto.Title}' already exists."); 
        }
        var category = _mapper.Map<CategoryOfEvent>(requestDto);
        
        var id = await _unitOfWork.Categories.AddAsync(category);
        
        await _unitOfWork.Complete();
        
        return id;
    }
}