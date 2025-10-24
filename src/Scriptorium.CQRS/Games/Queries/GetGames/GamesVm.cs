using System.Collections.Generic;

namespace Scriptorium.CQRS.Games.Queries.GetGames;

public class GamesVm
{
    public List<GameDto> Games { get; set; } = new();
}
