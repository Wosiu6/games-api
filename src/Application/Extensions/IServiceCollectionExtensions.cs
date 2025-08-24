using Application.EmailTokenVerification.Factories;
using AutoMapper;
using Domain.Identity.IdentityProviders;
using Domain.Identity.PasswordHashers;
using Games.CQRS.Commands.CreateGame;
using Games.CQRS.Commands.DeleteGame;
using Games.CQRS.Commands.UpdateGame;
using Games.CQRS.Queries.GetGames;
using Identity.CQRS.Commands.CreateUser;
using Identity.CQRS.Commands.LoginUser;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Application.Extensions
{
    internal static class IServiceCollectionExtensions
    {
        
        internal static void AddGamesServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { }, typeof(GameDto.Mapping));
        }

        internal static void AddIdentityServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { }, typeof(UserDto.Mapping));
            services.AddAutoMapper(cfg => { }, typeof(UserVm.Mapping));

            services.AddSingleton<PasswordHasher>();
            services.AddSingleton<TokenProvider>();
            services.AddScoped<EmailVerificationLinkFactory>();
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

        internal static void ConfigureUsersEndpoints(this WebApplication app)
        {
            app.MapPost("/users", async (IMediator mediator, CreateUserCommand request) =>
            {
                var newUser = await mediator.Send(request);
                return TypedResults.Created($"users/{newUser.Id}", newUser);
            });

            app.MapPost("/users/login", async (IMediator mediator, LoginUserCommand request) =>
            {
                var token = await mediator.Send(request);
                return TypedResults.Created($"users/login/", new { jwt_token = token });
            });
        }
    }
}
