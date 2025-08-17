using System.Security.Claims;
using Application.EmailVerificationTokens.Factory;
using AutoMapper;
using Domain.Identity.IdentityProviders;
using Domain.Identity.PasswordHashers;
using Games.Games.Commands.CreateGame;
using Games.Games.Commands.DeleteGame;
using Games.Games.Commands.UpdateGame;
using Games.Games.Queries.GetGames;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Users.Users.Commands.CreateUser;

namespace Application;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(GameDto.Mapping));
    }

    public static void MapGamesEndpoints(this WebApplication builder)
    {
        builder.ConfigureGamesEndpoints();
    }
    
    public static void AddIdentityServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(UserDto.Mapping));
        services.AddAutoMapper(cfg => { }, typeof(UserVm.Mapping));
        
        services.AddSingleton<PasswordHasher>();
        services.AddSingleton<TokenProvider>();
        services.AddScoped<EmailVerificationLinkFactory>();
        services.AddScoped<Mapper>();
        services.AddHttpContextAccessor();
    }

    public static void MapIdentityEndpoints(this WebApplication builder)
    {
        builder.ConfigureUsersEndpoints();
    }

    private static void ConfigureGamesEndpoints(this WebApplication app)
    {
        app.MapPost("/games", async (IMediator mediator, CreateGameCommand request) =>
        {
            var newGameId = await mediator.Send(request);
            return TypedResults.Created($"games/{newGameId}", newGameId);
        }).RequireAuthorization();
        
        app.MapPut("/games/{id:int}", async Task<IResult> (IMediator mediator, int id, UpdateGameCommand request) =>
        {
            if (id != request.Id) return TypedResults.BadRequest();
            await mediator.Send(request);
            return TypedResults.NoContent();
        });

        app.MapDelete("/games/{id:int}", async (IMediator mediator, int id) =>
        {
            await mediator.Send(new DeleteGameCommand(id));
            return TypedResults.NoContent();
        });

        app.MapGet("/games", async (IMediator mediator, ClaimsPrincipal user) =>
        {
            string userId = user.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            
            return TypedResults.Ok(await mediator.Send(new GetGamesQuery()));
        }).RequireAuthorization();
    }

    private static void ConfigureUsersEndpoints(this WebApplication app)
    {
        app.MapPost("/users", async (IMediator mediator, CreateUserCommand request) =>
        {
            var newUser = await mediator.Send(request);
            return TypedResults.Created($"users/{newUser.Id}", newUser);
        });
    }
}