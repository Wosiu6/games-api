using AutoMapper;
using GamesApi.Domain.Entities;

namespace GamesApi.Application.Games.Queries.GetGames;

public class GamesVm
{
    public List<GameDto> Games { get; set; }
}