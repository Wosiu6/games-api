using Application.Games.Commands.CreateGame;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests;

public class CreateGameHandlerTests
{
    [Fact]
    public async Task CreateGameHandler_Creates_Game_In_Db()
    {
        var options = new DbContextOptionsBuilder<GamesDbContext>()
            .UseInMemoryDatabase(databaseName: "CreateGameTestDb")
            .Options;

        await using var context = new GamesDbContext(options);

        var handler = new CreateGameCommandHandler(context);

        var cmd = new CreateGameCommand
        {
            Title = "Unit Test Game",
            ReleaseDate = DateTime.UtcNow
        };

        int id = await handler.Handle(cmd, CancellationToken.None);

        var saved = await context.Games.FindAsync([id], CancellationToken.None);

        Assert.NotNull(saved);
        Assert.Equal("Unit Test Game", saved.Title);
    }
}
