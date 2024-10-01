using AutoMapper;
using EventApp.Application.DTOs.User;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;
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
        var existingUser = await _unitOfWork.Users.GetByEmail(request.UserEmail);
        if (existingUser != null)
        {
            throw new DuplicateUsers("A user with this email already exists");
        }

        var user = _mapper.Map<Core.Models.User>(request);
        user.Password = _passwordHasher.HashPassword(request.Password);

        await _unitOfWork.Users.Create(user);
        await _unitOfWork.Complete();
    }
}