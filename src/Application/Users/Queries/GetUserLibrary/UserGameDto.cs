using AutoMapper;
using Domain.Entities;

namespace Application.Users.Queries.GetUserLibrary;

public class UserGameDto
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public int GameId { get; init; }
    public DateTime? OwnedAt { get; init; }
    public double PlayTimeHours { get; init; }
    public bool IsInstalled { get; init; }

    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserGame, UserGameDto>();
        }
    }
}
