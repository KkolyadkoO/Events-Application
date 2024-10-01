using AutoMapper;
using EventApp.Application.DTOs.User;
using EventApp.Core.Abstractions.Repositories;

namespace EventApp.Application.UseCases.User;

public class GetAllUsersUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllUsersUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<UsersResponseDto>> Execute()
    {
        var users = await _unitOfWork.Users.Get();
        return _mapper.Map<List<UsersResponseDto>>(users);
    }
}