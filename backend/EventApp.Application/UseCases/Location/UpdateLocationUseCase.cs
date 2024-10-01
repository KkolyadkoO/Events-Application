using AutoMapper;
using EventApp.Application.DTOs.LocationOfEvent;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;

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
        var location = await _unitOfWork.Locations.GetById(id);
        
        if (location == null)
        {
            throw new NotFoundException($"Location with ID '{id}' not found.");
        }
        location.Title = requestDto.Title;
        
        await _unitOfWork.Locations.Update(location);
        await _unitOfWork.Complete();

        return id;
    }
}