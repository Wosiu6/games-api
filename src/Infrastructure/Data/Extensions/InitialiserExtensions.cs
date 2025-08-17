using Domain.Common.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data.Extensions;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync<T>(this WebApplication app) where T : IDbContextInitialiser
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<T>();

        await initialiser.InitialiseAsync();
    }
}