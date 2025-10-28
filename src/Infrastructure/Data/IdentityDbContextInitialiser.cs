using Domain.Common.Interfaces;
using Domain.Entities;
using Domain.Identity.PasswordHashers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data;

public class IdentityDbContextInitialiser(
    ILogger<IdentityDbContextInitialiser> logger,
    IPasswordHasher<User> passwordHasher,
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
        string placeholderHash = new Guid().ToString();

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
                    PasswordHash = placeholderHash
                },
                new()
                {
                    Email = "john.doe@example.com",
                    FirstName = "John",
                    LastName = "Doe",
                    EmailVerified = true,
                    PasswordHash = placeholderHash
                },
                new()
                {
                    Email = "jane.smith@example.com",
                    FirstName = "Jane",
                    LastName = "Smith",
                    EmailVerified = false,
                    PasswordHash = placeholderHash
                },
                new()
                {
                    Email = "test@example.com",
                    FirstName = "Test",
                    LastName = "User",
                    EmailVerified = true,
                    PasswordHash = placeholderHash
                }
            };

            context.Users.AddRange(users);
            await context.SaveChangesAsync();

            users[0].PasswordHash = passwordHasher.HashPassword(users[0], "AdminPassword123£");
            users[1].PasswordHash = passwordHasher.HashPassword(users[1], "password123");
            users[2].PasswordHash = passwordHasher.HashPassword(users[2], "Password123");
            users[3].PasswordHash = passwordHasher.HashPassword(users[3], "Test123!");

            await context.SaveChangesAsync();
        }
    }
}