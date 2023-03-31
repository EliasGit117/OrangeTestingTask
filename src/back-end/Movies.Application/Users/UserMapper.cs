using AutoMapper;
using Movies.Application.Users.Commands;
using Movies.Domain.Dtos;
using Movies.Domain.Entities;

namespace Movies.Application.Users;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<CreateUserCmd, User>();
        CreateMap<User, UserDto>();
    }
}
