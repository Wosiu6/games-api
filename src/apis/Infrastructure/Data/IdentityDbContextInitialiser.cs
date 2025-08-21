using Domain.Common.Interfaces;
using Domain.Entities;
using Domain.Identity.PasswordHashers;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data;

public class IdentityDbContextInitialiser(
    ILogger<IdentityDbContextInitialiser> logger,
    PasswordHasher passwordHasher,
    IdentityDbContext context) : IDbContextInitialiser
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
        if (!context.Users.Any())
        {
            var users = new List<User>
            {
                new()
                {
                    Email = "admin@example.com",
                    FirstName = "Admin",
                    LastName = "User",
                    EmailVerified = true,
                    PasswordHash = passwordHasher.HashPassword("AdminPassword123£")
                },
                new()
                {
                    Email = "john.doe@example.com",
                    FirstName = "John",
                    LastName = "Doe",
                    EmailVerified = true,
                    PasswordHash = passwordHasher.HashPassword("password123")
                },
                new() {
                    Email = "jane.smith@example.com",
                    FirstName = "Jane",
                    LastName = "Smith",
                    EmailVerified = false,
                    PasswordHash = passwordHasher.HashPassword("Password123")
                },
                new() {
                    Email = "test@example.com",
                    FirstName = "Test",
                    LastName = "User",
                    EmailVerified = true,
                    PasswordHash = passwordHasher.HashPassword("Test123!")
                }
            };

            context.Users.AddRange(users);
            await context.SaveChangesAsync();
        }
    }
}