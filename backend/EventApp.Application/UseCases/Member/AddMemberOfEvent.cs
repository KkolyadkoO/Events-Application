using AutoMapper;
using EventApp.Application.DTOs.MemberOfEvent;
using EventApp.Core.Models;
using EventApp.DataAccess.Abstractions;

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
        await _unitOfWork.Members.AddAsync(memberOfEvent);
        await _unitOfWork.Complete();
        return memberOfEvent.Id;
    }
}