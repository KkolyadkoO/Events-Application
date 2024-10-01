using AutoMapper;
using EventApp.Application.DTOs.User;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;

namespace EventApp.Application.UseCases.User;

public class GetUserByIdUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserByIdUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UsersResponseDto> Execute(Guid id)
    {
        var user = await _unitOfWork.Users.GetById(id);
        if (user == null)
        {
            throw new UserNotFound($"User with ID {id} not found.");
        }

        return _mapper.Map<UsersResponseDto>(user);
    }
}