using AutoMapper;
using EventApp.Application.DTOs.User;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;
using EventApp.Infrastructure;

namespace EventApp.Application.UseCases.User;

public class LoginUserUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;

    public LoginUserUseCase(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public async Task<UsersResponseDto> Execute(UserLoginRequestDto request)
    {
        var user = await _unitOfWork.Users.GetByLogin(request.UserName);
        if (user == null || !_passwordHasher.VerifyHashedPassword(user.Password, request.Password))
        {
            throw new InvalidLoginException("Invalid username or password");
        }

        return _mapper.Map<UsersResponseDto>(user);
    }
}