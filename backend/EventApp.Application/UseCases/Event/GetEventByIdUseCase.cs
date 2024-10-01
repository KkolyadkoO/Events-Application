using AutoMapper;
using EventApp.Application.DTOs.Event;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;

namespace EventApp.Application.UseCases.Event;

public class GetEventByIdUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEventByIdUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<EventsResponseDto> Execute(Guid id)
    {
        var existingEvent = await _unitOfWork.Events.GetById(id);
        if (existingEvent == null)
        {
            throw new NotFoundException($"Event with id {id} not found");
        }
        
        return _mapper.Map<EventsResponseDto>(existingEvent);
    }
}