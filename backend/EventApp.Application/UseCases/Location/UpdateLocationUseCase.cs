using AutoMapper;
using EventApp.Application.DTOs.LocationOfEvent;
using EventApp.Application.Exceptions;
using EventApp.Core.Models;
using EventApp.DataAccess.Abstractions;

namespace EventApp.Application.UseCases.Location;

public class UpdateLocationUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateLocationUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Execute(Guid id, LocationOfEventsRequestDto requestDto)
    {
        var location = await _unitOfWork.Locations.GetByIdAsync(id);
        
        if (location == null)
        {
            throw new NotFoundException($"Location with ID '{id}' not found.");
        }
        var updateLocation = _mapper.Map<LocationOfEvent>(requestDto);
        updateLocation.Id = id;
        await _unitOfWork.Locations.UpdateAsync(updateLocation);
        await _unitOfWork.Complete();

        return id;
    }
}