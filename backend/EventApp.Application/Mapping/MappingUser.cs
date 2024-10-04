using AutoMapper;
using EventApp.Application.DTOs.Event;
using EventApp.Application.DTOs.User;
using EventApp.Core.Models;

namespace EventApp.Application.Mapping;

public class MappingUser : Profile
{
    public MappingUser()
    {
        CreateMap<UserRegisterRequestDto, User>()
            .ConstructUsing(src => new User(
                Guid.NewGuid(), 
                src.UserName,
                src.UserName,
                src.Password,
                src.Role
            ));
        CreateMap<User, UsersResponseDto>();
    }
}