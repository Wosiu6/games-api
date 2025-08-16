using AutoMapper;
using GamesApi.Domain.Entities;

namespace GamesApi.Application.Games.Queries.GetGames;

public class GameVm
{
    public int Id { get; init; }
    public string Title { get; init; }
    public DateTime ReleaseDate { get; init; }
    public DateTime CreatedOn { get; init; }
    public DateTime UpdatedOn { get; init; }
}