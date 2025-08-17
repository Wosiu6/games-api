using AutoMapper;
using Domain.Entities;

namespace Users.Users.Commands.CreateUser;

public class UserDto
{
    public int Id { get; init; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PasswordHash { get; set; }
    public bool EmailVerified { get; set; }
    public DateTime CreatedOn { get; init; }
    public DateTime UpdatedOn { get; init; }

    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<User, UserDto>();
        }
    }
}