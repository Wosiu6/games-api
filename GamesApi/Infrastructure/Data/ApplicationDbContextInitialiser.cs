using GamesApi.Application.Common.Interfaces;
using GamesApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GamesApi.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser(
    ILogger<ApplicationDbContextInitialiser> logger,
    ApplicationDbContext context)
{
    public async Task InitialiseAsync()
    {
        try
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            await SeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }
    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        if (!context.Games.Any())
        {
            var games = new List<Game>
            {
                new Game()
                {
                    Title = "Cyberpunk 2077",
                    ReleaseDate = new DateTime(2020, 12, 10, 0, 0, 0, DateTimeKind.Utc),
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow
                },
                new Game()
                {
                    Title = "The Witcher",
                    ReleaseDate = new DateTime(2007, 10, 26, 0, 0, 0, DateTimeKind.Utc),
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow
                },
                new Game()
                {
                    Title = "The Witcher 2: Assassins of Kings",
                    ReleaseDate = new DateTime(2011, 5, 17, 0, 0, 0, DateTimeKind.Utc),
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow
                },
                new Game()
                {
                    Title = "The Witcher 3: Wild Hunt",
                    ReleaseDate = new DateTime(2015, 5, 19, 0, 0, 0, DateTimeKind.Utc),
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow
                }
            };

            context.Games.AddRange(games);
            await context.SaveChangesAsync();
        }
    }
}