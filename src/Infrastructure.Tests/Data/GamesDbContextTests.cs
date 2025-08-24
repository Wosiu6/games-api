using Domain.Common.Interfaces;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Data;

public class GamesDbContextTests : IDisposable
{
    private readonly DbContextOptions<GamesDbContext> _options;
    private readonly GamesDbContext _context;

    public GamesDbContextTests()
    {
        _options = new DbContextOptionsBuilder<GamesDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new GamesDbContext(_options);
    }

    [Fact]
    public void Constructor_WithValidOptions_ShouldCreateContext()
    {
        var context = new GamesDbContext(_options);

        context.Should().NotBeNull();
        context.Should().BeAssignableTo<DbContext>();
        context.Should().BeAssignableTo<IGamesDbContext>();
    }

    [Fact]
    public void Games_Property_ShouldNotBeNull()
    {
        _context.Games.Should().NotBeNull();
        _context.Games.Should().BeAssignableTo<DbSet<Game>>();
    }

    [Fact]
    public void Context_ShouldImplementIGamesDbContext()
    {
        _context.Should().BeAssignableTo<IGamesDbContext>();
    }

    [Fact]
    public async Task Games_Add_ShouldAddGameToContext()
    {
        var game = SeedGame();

        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        var addedGame = await _context.Games.FirstOrDefaultAsync();
        addedGame.Should().NotBeNull();
        addedGame.Should().Be(game);
    }

    [Fact]
    public async Task Games_AddMultiple_ShouldAddAllGamesToContext()
    {
        var gameCount = 3;
        var games = SeedGames(count: gameCount);

        _context.Games.AddRange(games);
        await _context.SaveChangesAsync();

        var addedGames = await _context.Games.ToListAsync();
        addedGames.Should().HaveCount(gameCount);
        addedGames.Should().BeEquivalentTo(games);
    }

    [Fact]
    public async Task Games_Remove_ShouldRemoveGameFromContext()
    {
        var game = SeedGame();
        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        _context.Games.Remove(game);
        await _context.SaveChangesAsync();

        var removedGame = await _context.Games.FirstOrDefaultAsync();
        removedGame.Should().BeNull();
    }

    [Fact]
    public async Task Games_Update_ShouldUpdateGameInContext()
    {
        var originalTitle = "Original Game";
        var updatedTitle = "Updated Game Title";
        var game = SeedGame(title: originalTitle);
        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        _context.Entry(game).State = EntityState.Detached;

        game.Title = updatedTitle;
        _context.Games.Update(game);
        await _context.SaveChangesAsync();

        var updatedGame = await _context.Games.FirstOrDefaultAsync();
        updatedGame.Should().NotBeNull();
        updatedGame.Title.Should().Be(updatedTitle);
    }

    [Fact]
    public void Database_ShouldBeConfigured()
    {
        _context.Database.Should().NotBeNull();
        _context.Database.IsInMemory().Should().BeTrue();
    }

    [Fact]
    public void ChangeTracker_ShouldBeAccessible()
    {
        _context.ChangeTracker.Should().NotBeNull();
    }

    [Fact]
    public async Task SaveChangesAsync_WithNoChanges_ShouldReturnZero()
    {
        var result = await _context.SaveChangesAsync();

        result.Should().Be(0);
    }

    [Fact]
    public async Task SaveChangesAsync_WithOneAddedEntity_ShouldReturnOne()
    {
        var game = SeedGame();

        _context.Games.Add(game);
        var result = await _context.SaveChangesAsync();

        result.Should().Be(1);
    }

    [Fact]
    public void Context_ShouldHaveCorrectDatabaseProvider()
    {
        _context.Database.ProviderName.Should().Be("Microsoft.EntityFrameworkCore.InMemory");
    }

    private static Game SeedGame(string title = "Default Game Title", DateTime? releaseDate = null)
    {
        return new Game
        {
            Title = title,
            ReleaseDate = releaseDate ?? new DateTime(2024, 1, 1)
        };
    }

    private static List<Game> SeedGames(int count = 3)
    {
        var games = new List<Game>();
        var baseYear = 2024;
        for (int i = 1; i <= count; i++)
        {
            var gameTitle = $"Game {i}";
            var releaseDate = new DateTime(baseYear, i, 1);
            games.Add(SeedGame(title: gameTitle, releaseDate: releaseDate));
        }
        return games;
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
