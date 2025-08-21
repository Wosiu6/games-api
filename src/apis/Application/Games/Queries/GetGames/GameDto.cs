using AutoMapper;
using Domain.Entities;

namespace Application.Games.Queries.GetGames;

public class GameDto
{
    public int Id { get; init; }
    public string Title { get; init; }
    public DateTime ReleaseDate { get; init; }
    public DateTime CreatedOn { get; init; }
    public DateTime UpdatedOn { get; init; }

    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Game, GameDto>();
        }
    }
}