using AutoMapper;
using EventApp.Application.DTOs.LocationOfEvent;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;
using EventApp.Core.Models;

namespace EventApp.Application.UseCases.Location;

public class AddLocationUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddLocationUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Execute(LocationOfEventsRequestDto requestDto)
    {
        var existingLocation = await _unitOfWork.Locations.GetByTitle(requestDto.Title);
        if (existingLocation != null)
        {
            throw new DuplicateCategory($"Location with title '{requestDto.Title}' already exists."); 
        }
        var location = _mapper.Map<LocationOfEvent>(requestDto);
        
        var id = await _unitOfWork.Locations.Add(location);
        
        await _unitOfWork.Complete();
        
        return id;
    }
}