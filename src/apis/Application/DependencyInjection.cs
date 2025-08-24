using Application.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static void AddGamesApplicationServices(this IServiceCollection services)
    {
        services.AddGamesServices();
    }

    public static void AddIdentityApplicationServices(this IServiceCollection services)
    {
        services.AddIdentityServices();
    }

    public static void MapGamesEndpoints(this WebApplication builder)
    {
        builder.ConfigureGamesEndpoints();
    }

    public static void MapIdentityEndpoints(this WebApplication builder)
    {
        builder.ConfigureUsersEndpoints();
    }
}