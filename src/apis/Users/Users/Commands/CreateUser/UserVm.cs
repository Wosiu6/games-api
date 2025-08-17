using AutoMapper;
using Users.Users.Commands.CreateUser;

namespace Games.Games.Commands.UpdateGame;

public class UserVm
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
            CreateMap<UserDto, UserVm>();
        }
    }
}

