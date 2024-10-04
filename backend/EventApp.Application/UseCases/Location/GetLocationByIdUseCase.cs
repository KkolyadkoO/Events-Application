using AutoMapper;
using EventApp.Application.DTOs.LocationOfEvent;
using EventApp.Application.Exceptions;
using EventApp.DataAccess.Abstractions;

namespace EventApp.Application.UseCases.Location;

public class GetLocationByIdUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetLocationByIdUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LocationOfEventsResponseDto> Execute(Guid id)
    {
        var location = await _unitOfWork.Locations.GetByIdAsync(id);
        if (location == null)
        {
            throw new NotFoundException($"Location with ID {id} not found.");
        }
        return _mapper.Map<LocationOfEventsResponseDto>(location);
    }
}