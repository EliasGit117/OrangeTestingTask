using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Movies.Application.Common.Exceptions;
using Movies.Domain.Entities;
using Movies.Infrastructure.Persistance;
using Movies.Infrastructure.Repositories;
using Movies.Infrastructure.Repositories.Interfaces;

namespace Movies.Application.Users.Commands;

public class LoginCmd: IRequest<User>
{
    public string Name { get; set; }
    public string Password { get; set; }
}

public class LoginCmdHandler : IRequestHandler<LoginCmd, User>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepo;

    public LoginCmdHandler(IMapper mapper,  IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepo = userRepository;
    }


    public async Task<User> Handle(LoginCmd request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetByName(request.Name, cancellationToken);
        
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            throw new BadRequestException("Wrong name or password");
        
        return user;
    }
}

public class LoginCmdValidator : AbstractValidator<LoginCmd>
{
    private const int MinNameLength = 5;
    private const int MaxNameLength = 100;
    
    private const int MinPasswordLength = 8;
    private const int MaxPasswordLength = 100;
    
    public LoginCmdValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(MinNameLength).WithMessage($"Name should have at least {MinNameLength} of symbols")
            .MaximumLength(MaxNameLength).WithMessage($"Name can't have more than {MaxNameLength} of symbols");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(MinPasswordLength).WithMessage($"Password should have at least {MinNameLength} of symbols")
            .MaximumLength(MaxPasswordLength).WithMessage($"Password can't have more than {MaxNameLength} of symbols")
            .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.");
    }
}