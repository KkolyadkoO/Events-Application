using AutoMapper;
using EventApp.Application.DTOs.Event;
using EventApp.Core.Models;

namespace EventApp.Application.Mapping;

public class MappingEvent : Profile
{
    public MappingEvent()
    {
        CreateMap<EventsRequestDto, Event>()
            .ConstructUsing(src => new Event(
                Guid.NewGuid(), 
                src.Title,
                src.Description,
                src.Date,
                src.LocationId,
                src.CategoryId,
                src.MaxNumberOfMembers,
                null
            ));
        CreateMap<Event, EventsResponseDto>()
            .ForMember(dest => dest.NumberOfMembers,
                opt =>
                    opt.MapFrom(src => src.MaxNumberOfMembers - src.Members.Count));
    }
}