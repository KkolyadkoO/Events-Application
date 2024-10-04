using AutoMapper;
using EventApp.Application.DTOs.LocationOfEvent;
using EventApp.Core.Models;

namespace EventApp.Application.Mapping;

public class MappingLocation : Profile
{
    public MappingLocation()
    {
        CreateMap<LocationOfEventsRequestDto, LocationOfEvent>()
            .ForMember(dest => dest.Id,
                opt =>
                    opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.Title,
                opt =>
                    opt.MapFrom(src => src.Title));
        CreateMap<LocationOfEvent, LocationOfEventsResponseDto>();
    }
}