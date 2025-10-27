using Application;
using Application.Identity.Commands.CreateUser;
using Application.Users.Commands.AddUserGame;
using Domain.Common.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Text;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureGamesDatabaseContext(configuration);
        services.ConfigureIdentityDatabaseContext(configuration);
        services.ConfigureMediatR();
        services.AddEmailServices(configuration);
    }

    private static void AddGameInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureGamesDatabaseContext(configuration);
        services.ConfigureIdentityDatabaseContext(configuration);
        services.ConfigureMediatR();
    }

    private static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureGamesDatabaseContext(configuration);
        services.ConfigureIdentityDatabaseContext(configuration);
        services.ConfigureMediatR();

        services.AddEmailServices(configuration);
    }

    public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = configuration["JwtSettings:Issuer"],
                ValidAudience = configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });
    }

    public static void AddOpenApiDocument(this IServiceCollection services, IConfiguration configuration, string apiName)
{
    services.AddOpenApiDocument(doc =>
    {
        doc.DocumentName = "v1";
        doc.PostProcess = d =>
        {
            d.Info.Version = "v1";
            d.Info.Title = $"{apiName} API v1.0";
        };
        
        doc.AddSecurity("bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
        });
        
        doc.OperationProcessors.Add(
            new OperationSecurityScopeProcessor("bearer"));
    });
}

    private static void AddEmailServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
        .AddFluentEmail(configuration["Email:SenderEmail"], configuration["Email:Sender"])
        .AddSmtpSender(configuration["Email:Host"], configuration.GetValue<int>("Email:Port"));
    }

    private static void ConfigureGamesDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<GamesDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("GamesDb")));

        services.AddScoped<IGamesDbContext>(provider =>
            provider.GetRequiredService<GamesDbContext>());

        services.AddScoped<GamesDbContextInitialiser>();
    }

    private static void ConfigureIdentityDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Identity")));

        services.AddScoped<IIdentityDbContext>(provider =>
            provider.GetRequiredService<IdentityDbContext>());

        services.AddScoped<IdentityDbContextInitialiser>();
    }

    private static void ConfigureMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(UserVm).Assembly);
        });
    }
}