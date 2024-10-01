using AutoMapper;
using EventApp.Application.DTOs.MemberOfEvent;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;

namespace EventApp.Application.UseCases.Member;

public class GetMemberOfEventById
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMemberOfEventById(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<MemberOfEventsResponseDto> Execute(Guid id)
    {
        var memberOfEvent = await _unitOfWork.Members.GetById(id);
        if (memberOfEvent == null)
        {
            throw new NotFoundException($"Member of Event with Id {id} not found");
        }

        return _mapper.Map<MemberOfEventsResponseDto>(memberOfEvent);
    }
}