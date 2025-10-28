using Application.EmailTokenVerification.Factories;
using AutoMapper;
using Domain.Identity.IdentityProviders;
using Domain.Identity.PasswordHashers;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Application.Games.Commands.CreateAchievement;
using Application.Games.Commands.CreateGame;
using Application.Games.Commands.UpdateGame;
using Application.Games.Queries.GetAchievements;
using Application.Games.Queries.GetGames;
using Application.Identity.Commands.CreateUser;
using Application.Identity.Commands.LoginUser;
using Application.Users.Commands.AddUserGame;
using Application.Users.Queries.GetUserLibrary;
using Application.Games.Commands.DeleteGame;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using Domain.Common.Interfaces;
using Application.Games.Commands.UpdateAchievement;
using Application.Games.Commands.ProgressAchievement;
using Application.Identity.Queries.GetUsers;

namespace Application.Extensions
{
    internal static class IServiceCollectionExtensions
    {
        
        internal static void AddGamesServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { },
                typeof(GameDto.Mapping),
                typeof(AchievementDto.Mapping));
        }

        internal static void AddIdentityServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { }, typeof(UserDto.Mapping));
            services.AddAutoMapper(cfg => { }, typeof(UserVm.Mapping));

            services.AddSingleton<TokenProvider>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<Domain.Common.Interfaces.IEmailVerificationLinkFactory, EmailVerificationLinkFactory>();
            services.AddScoped<Mapper>();
            services.AddHttpContextAccessor();
        }

    internal static void ConfigureGamesEndpoints(this WebApplication app)
        {
            app.MapPost("/games", async (IMediator mediator, CreateGameCommand request) =>
            {
                var newGameId = await mediator.Send(request);
                return TypedResults.Created($"games/{newGameId}", newGameId);
            });

            app.MapPost("/games/{gameId:int}/achievements", async Task<IResult> (IMediator mediator, CreateAchievementCommand request, int gameId) =>
            {
                if (request == null) return TypedResults.BadRequest();
                if (gameId != request.GameId) return TypedResults.BadRequest();
                var achievementId = await mediator.Send(request);
                return TypedResults.Created($"games/{gameId}/achievements/{achievementId}", achievementId);
            });

            app.MapGet("/games/{gameId:int}/achievements", async (IMediator mediator, int gameId) =>
            {
                var vm = await mediator.Send(new GetAchievementsQuery(gameId));
                return TypedResults.Ok(vm);
            });

            app.MapPut("/games/{gameId:int}/achievements/{achievementId:int}", async Task<IResult> (IMediator mediator, int gameId, int achievementId, UpdateAchievementCommand request) =>
            {
                if (achievementId != request.AchievementId) return TypedResults.BadRequest();
                if (gameId != request.GameId) return TypedResults.BadRequest();
                await mediator.Send(request);
                return TypedResults.NoContent();
            });

            app.MapPut("/games/{gameId:int}/achievements/{achievementId:int}/progress", async Task<IResult> (IMediator mediator, int gameId, int achievementId) =>
            {
                await mediator.Send(new ProgressAchievementCommand(achievementId, gameId));
                return TypedResults.NoContent();
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
            }).RequireAuthorization();

            app.MapGet("/games", async (IMediator mediator, ClaimsPrincipal user) =>
            {
                return TypedResults.Ok(await mediator.Send(new GetGamesQuery()));
            });
        }

        internal static void ConfigureUserLibraryEndpoints(this WebApplication app)
        {
            app.MapPost("/users/{userId:int}/library", async Task<IResult> (IMediator mediator, AddUserGameCommand request, int userId) =>
            {
                if (request == null) return TypedResults.BadRequest();
                if (userId != request.UserId) return TypedResults.BadRequest();
                var id = await mediator.Send(request);
                return TypedResults.Created($"users/{userId}/library/{id}", id);
            }).RequireAuthorization();

            app.MapGet("/users/{userId:int}/library", async (IMediator mediator, int userId) =>
            {
                var vm = await mediator.Send(new GetUserLibraryQuery(userId));
                return TypedResults.Ok(vm);
            }).RequireAuthorization();
        }

        internal static void ConfigureUsersEndpoints(this WebApplication app)
        {
            app.MapPost("/users", async (IMediator mediator, CreateUserCommand request) =>
            {
                var newUserId = await mediator.Send(request);
                return TypedResults.Created($"users/{newUserId}");
            });

            app.MapPost("/users/login", async (IMediator mediator, LoginUserCommand request) =>
            {
                var token = await mediator.Send(request);
                return TypedResults.Created($"users/login/", new { jwt_token = token });
            });

            app.MapGet("/users", async (IMediator mediator) =>
            {
                var vm = await mediator.Send(new GetUsersQuery());
                return TypedResults.Ok(vm);
            }).RequireAuthorization();
        }
    }
}
