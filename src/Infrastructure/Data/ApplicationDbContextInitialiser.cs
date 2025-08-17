using Domain.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data;

public class ApplicationDbContextInitialiser(
    ILogger<ApplicationDbContextInitialiser> logger,
    ApplicationDbContext context) : IDbContextInitialiser
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