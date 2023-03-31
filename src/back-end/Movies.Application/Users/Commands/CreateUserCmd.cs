using AutoMapper;
using Azure.Core;
using FluentValidation;
using MediatR;
using Movies.Domain.Dtos;
using Movies.Domain.Entities;
using Movies.Infrastructure.Persistance;
using Movies.Infrastructure.Repositories;
using Movies.Infrastructure.Repositories.Interfaces;

namespace Movies.Application.Users.Commands;

public class CreateUserCmd: IRequest
{
    public string Name { get; set; }
    public string Password { get; set; }
}

public class CreateUserCmdHandler : IRequestHandler<CreateUserCmd>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepo;

    public CreateUserCmdHandler(IMapper mapper, IUserRepository userRepo)
    {
        _mapper = mapper;
        _userRepo = userRepo;
    }


    public async Task Handle(CreateUserCmd request, CancellationToken cancellationToken)
    {
        request.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = _mapper.Map<User>(request);
        await _userRepo.Insert(user, cancellationToken);
    }
}

public class CreateUserCmdValidator : AbstractValidator<CreateUserCmd>
{
    private const int MinNameLength = 5;
    private const int MaxNameLength = 100;
    
    private const int MinPasswordLength = 8;
    private const int MaxPasswordLength = 100;

    public CreateUserCmdValidator(IUserRepository userRepository)
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
        
        RuleFor(x => x.Name).MustAsync(async (x, id, token) => 
        {
            var exists = await userRepository.CheckNameUniqueness(x.Name, token);
            return !exists;
        }).WithMessage("User with such name already exists");
    }
}