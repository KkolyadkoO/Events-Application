using AutoMapper;
using EventApp.Application.DTOs.MemberOfEvent;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Models;

namespace EventApp.Application.UseCases.Member;

public class AddMemberOfEvent
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddMemberOfEvent(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Execute(MemberOfEventsRequestDto requestDto)
    {
        var memberOfEvent = _mapper.Map<MemberOfEvent>(requestDto); 
        await _unitOfWork.Members.Create(memberOfEvent);
        await _unitOfWork.Complete();
        return memberOfEvent.Id;
    }
}