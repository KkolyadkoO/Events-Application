using AutoMapper;
using EventApp.Application.DTOs.Event;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Specifications;

namespace EventApp.Application.UseCases.Event;

public class GetEventsByFiltersUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEventsByFiltersUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<(List<EventsResponseDto>, int)> Execute(EventFilterRequestDto request)
    {
        var specification = new EventSpecification(request.Title, request.LocationId,
            request.StartDate, request.EndDate, request.Category, request.UserId);
        var result = await _unitOfWork.Events.GetBySpecificationAsync(specification, request.Page, request.PageSize);
        return (_mapper.Map<List<EventsResponseDto>>(result.Item1), result.Item2);
    }
}