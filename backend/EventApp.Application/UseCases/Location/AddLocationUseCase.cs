using AutoMapper;
using EventApp.Application.DTOs.LocationOfEvent;
using EventApp.Application.Exceptions;
using EventApp.Core.Models;
using EventApp.DataAccess.Abstractions;

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
        var existingLocation = await _unitOfWork.Locations.GetByTitleAsync(requestDto.Title);
        if (existingLocation != null)
        {
            throw new DuplicateCategory($"Location with title '{requestDto.Title}' already exists."); 
        }
        var location = _mapper.Map<LocationOfEvent>(requestDto);
        
        var id = await _unitOfWork.Locations.AddAsync(location);
        
        await _unitOfWork.Complete();
        
        return id;
    }
}