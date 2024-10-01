using AutoMapper;
using EventApp.Application.DTOs.CategoryOfEvent;
using EventApp.Core.Abstractions.Repositories;

namespace EventApp.Application.UseCases.Category;

public class GetAllCategoriesUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllCategoriesUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<CategoryOfEventsResponseDto>> Execute()
    {
        var categories = await _unitOfWork.Categories.Get();
            
        return _mapper.Map<List<CategoryOfEventsResponseDto>>(categories);
    }
}