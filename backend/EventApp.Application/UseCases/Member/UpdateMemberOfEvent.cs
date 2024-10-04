using AutoMapper;
using EventApp.Application.DTOs.MemberOfEvent;
using EventApp.Application.Exceptions;
using EventApp.Core.Models;
using EventApp.DataAccess.Abstractions;

namespace EventApp.Application.UseCases.Member;

public class UpdateMemberOfEvent
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateMemberOfEvent(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Execute(Guid id, MemberOfEventsRequestDto requestDto)
    {
        var memberOfEvent = await _unitOfWork.Members.GetByIdAsync(id);
        if (memberOfEvent == null)
        {
            throw new NotFoundException($"Member of Event with Id {id} not found");
        }
        var updatedMemberOfEvent = _mapper.Map<MemberOfEvent>(requestDto);
        updatedMemberOfEvent.Id = id;

        await _unitOfWork.Members.UpdateAsync(updatedMemberOfEvent);
        await _unitOfWork.Complete();
    }
}