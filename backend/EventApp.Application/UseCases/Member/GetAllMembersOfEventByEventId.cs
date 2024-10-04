using AutoMapper;
using EventApp.Application.DTOs.MemberOfEvent;
using EventApp.DataAccess.Abstractions;

namespace EventApp.Application.UseCases.Member;

public class GetAllMembersOfEventByEventId
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllMembersOfEventByEventId(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<MemberOfEventsResponseDto>> Execute(Guid eventId)
    {
        var members = await _unitOfWork.Members.GetByEventIdAsync(eventId);
        return _mapper.Map<List<MemberOfEventsResponseDto>>(members);  
    }
}