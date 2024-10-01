using AutoMapper;
using EventApp.Application.DTOs.LocationOfEvent;
using EventApp.Core.Abstractions.Repositories;

namespace EventApp.Application.UseCases.Location;

public class GetAllLocationsUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllLocationsUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<LocationOfEventsResponseDto>> Execute()
    {
        var categories = await _unitOfWork.Locations.Get();
            
        return _mapper.Map<List<LocationOfEventsResponseDto>>(categories);
    }
}