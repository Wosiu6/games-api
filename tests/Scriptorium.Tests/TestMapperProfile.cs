using Application.Identity.Commands.CreateUser;
using AutoMapper;
using Domain.Entities;

namespace Scriptorium.Tests;

public class TestMapperProfile : Profile
{
    public TestMapperProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<User, UserVm>();
    }
}