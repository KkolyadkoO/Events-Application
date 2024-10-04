using AutoMapper;
using EventApp.Application.DTOs.Event;
using EventApp.Application.Exceptions;
using EventApp.DataAccess.Abstractions;

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
        var existingEvent = await _unitOfWork.Events.GetByIdAsync(id);
        if (existingEvent == null)
        {
            throw new NotFoundException($"Event with id {id} not found");
        }

        var response = _mapper.Map<EventsResponseDto>(existingEvent);
        response = response with { NumberOfMembers = existingEvent.Members.Count };
        return response;
    }
}