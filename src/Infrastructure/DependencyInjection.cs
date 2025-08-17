using Domain.Common.Interfaces;
using Games.Games.Commands.CreateGame;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Users.Users.Commands.CreateUser;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void ConfigureGamesDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("GamesDb")));
        
        services.AddScoped<IApplicationDbContext>(provider => 
            provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();
    }
    
    public static void ConfigureIdentityDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Identity")));
        
        services.AddScoped<IIdentityDbContext>(provider => 
            provider.GetRequiredService<IdentityDbContext>());
        
        services.AddScoped<IdentityDbContextInitialiser>();
    }
    
    public static void ConfigureGamesMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(typeof(CreateGameCommand).Assembly);
        });
    }
    
    public static void ConfigureIdentityMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(typeof(CreateUserCommand).Assembly);
        });
    }
}