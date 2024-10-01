using AutoMapper;
using EventApp.Application.DTOs.MemberOfEvent;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;

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
        var memberOfEvent = await _unitOfWork.Members.GetById(id);
        if (memberOfEvent == null)
        {
            throw new NotFoundException($"Member of Event with Id {id} not found");
        }

        await _unitOfWork.Members.Update(_mapper.Map(requestDto, memberOfEvent));
        await _unitOfWork.Complete();
    }
}