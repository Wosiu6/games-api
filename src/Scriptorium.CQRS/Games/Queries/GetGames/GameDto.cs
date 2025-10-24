using AutoMapper;
using Domain.Entities;

namespace Scriptorium.CQRS.Games.Queries.GetGames;

public class GameDto
{
    public int Id { get; init; }
    public string Title { get; init; } = null!;
    public DateTime ReleaseDate { get; init; }
    public string? Description { get; init; }
    public string? Genre { get; init; }
    public string? Developer { get; init; }
    public string? Publisher { get; init; }
    public decimal Price { get; init; }
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
