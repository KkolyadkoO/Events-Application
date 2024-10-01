using AutoMapper;
using EventApp.Application.DTOs.CategoryOfEvent;
using EventApp.Application.DTOs.LocationOfEvent;
using EventApp.Application.DTOs.MemberOfEvent;
using EventApp.Core.Models;

namespace EventApp.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapping for CategoryOfEvent
        CreateMap<CategoryOfEventsRequestDto, CategoryOfEvent>()
            .ForMember(dest => dest.Id,
                opt =>
                    opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.Title,
                opt =>
                    opt.MapFrom(src => src.Title));
        CreateMap<CategoryOfEvent, CategoryOfEventsResponseDto>();
        
        // Mapping for LocationOfEvent
        CreateMap<LocationOfEventsRequestDto, LocationOfEvent>()
            .ForMember(dest => dest.Id,
                opt =>
                    opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.Title,
                opt =>
                    opt.MapFrom(src => src.Title));
        CreateMap<LocationOfEvent, LocationOfEventsResponseDto>();
        
        // Mapping for MemberOfEvent
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