using Domain.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data;

public class IdentityDbContextInitialiser(
    ILogger<IdentityDbContextInitialiser> logger,
    IdentityDbContext context) : IDbContextInitialiser
{
    public async Task InitialiseAsync()
    {
        try
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }
    
}