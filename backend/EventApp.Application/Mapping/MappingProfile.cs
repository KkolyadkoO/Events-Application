using AutoMapper;
using EventApp.Application.DTOs.CategoryOfEvent;
using EventApp.Core.Models;

namespace EventApp.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CategoryOfEventsRequestDto, CategoryOfEvent>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title));
        CreateMap<CategoryOfEvent, CategoryOfEventsResponseDto>();
    }
}