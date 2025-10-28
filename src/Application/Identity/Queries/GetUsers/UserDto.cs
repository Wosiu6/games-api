using AutoMapper;
using Domain.Entities;

namespace Application.Identity.Queries.GetUsers;

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
