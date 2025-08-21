using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Games.Queries.GetGames;

public record GetGamesQuery : IRequest<GamesVm>;

public class GetGamesQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetGamesQuery, GamesVm>
{
    public async Task<GamesVm> Handle(GetGamesQuery request, CancellationToken cancellationToken)
    {
        var gamesDtos = await context.Games
            .ProjectTo<GameDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        
        return new GamesVm()
        {
             Games = gamesDtos
        };
    }
}