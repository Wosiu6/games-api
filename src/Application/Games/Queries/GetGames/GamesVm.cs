using System.Collections.Generic;

namespace Application.Games.Queries.GetGames;

public class GamesVm
{
    public List<GameDto> Games { get; set; } = new();
}
