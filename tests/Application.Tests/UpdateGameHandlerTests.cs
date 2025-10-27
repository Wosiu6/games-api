using Application.Games.Commands.UpdateGame;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests;

public class UpdateGameHandlerTests
{
    [Fact]
    public async Task UpdateGameHandler_Updates_Fields()
    {
        var options = new DbContextOptionsBuilder<GamesDbContext>()
            .UseInMemoryDatabase(databaseName: "UpdateGameTestDb")
            .Options;

        await using var context = new GamesDbContext(options);

        var game = new Domain.Entities.Game
        {
            Title = "Old Title",
            ReleaseDate = DateTime.UtcNow,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };

        context.Games.Add(game);
        await context.SaveChangesAsync(CancellationToken.None);

        var handler = new UpdateGameCommandHandler(context);

        var cmd = new UpdateGameCommand(game.Id)
        {
            Title = "New Title",
            ReleaseDate = game.ReleaseDate
        };

        await handler.Handle(cmd, CancellationToken.None);

        var updated = await context.Games.FindAsync([game.Id], CancellationToken.None);

        Assert.Equal("New Title", updated.Title);
    }
}
