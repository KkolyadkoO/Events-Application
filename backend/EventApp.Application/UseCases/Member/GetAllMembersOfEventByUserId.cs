using AutoMapper;
using EventApp.Application.DTOs.MemberOfEvent;
using EventApp.DataAccess.Abstractions;

namespace EventApp.Application.UseCases.Member;

public class GetAllMembersOfEventByUserId
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllMembersOfEventByUserId(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<MemberOfEventsResponseDto>> Execute(Guid userId)
    {
        var members = await _unitOfWork.Members.GetByUserIdAsync(userId);
        return _mapper.Map<List<MemberOfEventsResponseDto>>(members);  
    }
}