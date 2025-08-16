using System.Reflection;
using AutoMapper;
using GamesApi.Application.Games.Commands.CreateGame;
using GamesApi.Application.Games.Commands.DeleteGame;
using GamesApi.Application.Games.Commands.UpdateGame;
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
            var newGameId = await mediator.Send(request);
            return TypedResults.Created($"games/{newGameId}", newGameId);
        });
        
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

        app.MapGet("/games", async (IMediator mediator) => TypedResults.Ok(await mediator.Send(new GetGamesQuery())));
    }
}