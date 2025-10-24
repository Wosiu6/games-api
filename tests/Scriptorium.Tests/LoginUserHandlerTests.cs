using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Identity.Commands.LoginUser;
using Domain.Identity.IdentityProviders;
using Domain.Identity.PasswordHashers;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Scriptorium.Tests;

public class LoginUserHandlerTests
{
    [Fact]
    public async Task LoginUserHandler_Returns_Token_For_Valid_Credentials()
    {
        var options = new DbContextOptionsBuilder<IdentityDbContext>()
            .UseInMemoryDatabase(databaseName: "LoginUserTestDb")
            .Options;

        await using var context = new IdentityDbContext(options);

        var passwordHasher = new PasswordHasher();
        var pwd = "MySecret123!";
        var user = new Domain.Entities.User
        {
            Email = "login@test.local",
            FirstName = "Login",
            LastName = "User",
            PasswordHash = passwordHasher.HashPassword(pwd),
            CreatedOn = System.DateTime.UtcNow,
            UpdatedOn = System.DateTime.UtcNow,
            EmailVerified = true
        };

        context.Users.Add(user);
        await context.SaveChangesAsync(CancellationToken.None);

        var inMemorySettings = new Dictionary<string, string?> {
            { "JwtSettings:Key", new string('x', 32) },
            { "JwtSettings:Issuer", "test-issuer" },
            { "JwtSettings:Audience", "test-audience" },
            { "JwtSettings:ExpirationInMinutes", "60" }
        };

        IConfiguration cfg = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
        var tokenProvider = new TokenProvider(cfg);

        var handler = new LoginUserCommandHandler(context, passwordHasher, tokenProvider);

        var cmd = new LoginUserCommand { Email = "login@test.local", Password = pwd };

        string token = await handler.Handle(cmd, CancellationToken.None);

        Assert.False(string.IsNullOrWhiteSpace(token));
    }
}
