using AutoMapper;
using EventApp.Application.DTOs.Event;
using EventApp.Application.DTOs.MemberOfEvent;
using EventApp.Application.DTOs.User;
using EventApp.Core.Models;

namespace EventApp.Application.Mapping;

public class MappingMemberOfEvent : Profile
{
    public MappingMemberOfEvent()
    {
        CreateMap<MemberOfEventsRequestDto, MemberOfEvent>()
            .ConstructUsing(src => new MemberOfEvent(Guid.NewGuid(),
                src.Name,
                src.LastName,
                src.Birthday,
                DateTime.Now.ToUniversalTime(),
                src.Email,
                src.UserId,
                src.EventId
            ));
        CreateMap<MemberOfEvent, MemberOfEventsResponseDto>();
    }
}