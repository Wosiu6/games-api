using AutoMapper;
using Domain.Entities;

namespace Scriptorium.CQRS.Identity.Commands.CreateUser;

public class UserDto
{
    public int Id { get; init; }
    public string Email { get; init; } = null!;
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;

    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<User, UserDto>();
        }
    }
}
