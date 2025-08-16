using GamesApi.Application.Common.Interfaces;
using GamesApi.Application.Games.Commands.CreateGame;
using GamesApi.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GamesApi.Infrastructure;

public static class DependencyInjection
{
    public static void ConfigureDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("GamesDb")));
        
        services.AddScoped<IApplicationDbContext>(provider => 
            provider.GetRequiredService<ApplicationDbContext>());
    }
    
    public static void ConfigureMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
    }
}