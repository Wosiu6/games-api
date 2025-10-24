using Application.Identity.Commands.CreateUser;
using AutoMapper;
using Domain.Entities;

namespace Application.Tests;

public class TestMapperProfile : Profile
{
    public TestMapperProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<User, UserVm>();
    }
}