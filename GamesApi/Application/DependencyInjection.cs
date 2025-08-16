using System.Reflection;
using AutoMapper;
using GamesApi.Application.Games.Commands.CreateGame;
using GamesApi.Application.Games.Queries.GetGames;
using GamesApi.Domain.Entities;
using MediatR;

namespace GamesApi.Application;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(GameDto.Mapping));
    }

    public static void MapEndpoints(this WebApplication builder)
    {
        builder.ConfigureGamesEndpoints();
    }

    private static void ConfigureGamesEndpoints(this WebApplication app)
    {
        app.MapPost("/games", async (IMediator mediator, CreateGameCommand request) =>
        {
            var newGame = await mediator.Send(request);
            return Results.Created($"games/{newGame}", newGame);
        });

        app.MapGet("/games", async (IMediator mediator) => Results.Ok(await mediator.Send(new GetGamesQuery())));
    }
}