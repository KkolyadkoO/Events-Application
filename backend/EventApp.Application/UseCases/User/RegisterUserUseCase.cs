using AutoMapper;
using EventApp.Application.DTOs.User;
using EventApp.Application.Exceptions;
using EventApp.DataAccess.Abstractions;
using EventApp.Infrastructure;

namespace EventApp.Application.UseCases.User;

public class RegisterUserUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;

    public RegisterUserUseCase(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public async Task Execute(UserRegisterRequestDto request)
    {
        var existingUserEmail = await _unitOfWork.Users.GetByEmailAsync(request.UserEmail);
        if (existingUserEmail != null)
        {
            throw new DuplicateUsers("A user with this email already exists");
        }
        var existingUserLogin = await _unitOfWork.Users.GetByLoginAsync(request.UserName);
        if (existingUserLogin != null)
        {
            throw new DuplicateUsers("A user with this login already exists");
        }

        var user = _mapper.Map<Core.Models.User>(request);
        user.Password = _passwordHasher.HashPassword(request.Password);

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.Complete();
    }
}