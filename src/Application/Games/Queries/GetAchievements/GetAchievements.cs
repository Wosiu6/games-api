using AutoMapper.QueryableExtensions;
using Domain.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Games.Queries.GetAchievements;

public record GetAchievementsQuery(int GameId) : IRequest<AchievementsVm>;

public class GetAchievementsQueryHandler(IGamesDbContext context, AutoMapper.IMapper mapper) : IRequestHandler<GetAchievementsQuery, AchievementsVm>
{
    public async Task<AchievementsVm> Handle(GetAchievementsQuery request, CancellationToken cancellationToken)
    {
        var dtos = await context.Achievements
            .Where(a => a.GameId == request.GameId)
            .ProjectTo<AchievementDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new AchievementsVm { Achievements = dtos };
    }
}

public class AchievementsVm
{
    public List<AchievementDto> Achievements { get; set; } = new();
}
