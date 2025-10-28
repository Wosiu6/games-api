using Ardalis.GuardClauses;
using Domain.Common.Interfaces;
using MediatR;

namespace Application.Games.Commands.ProgressAchievement;

public record ProgressAchievementCommand(int AchievementId, int GameId) : IRequest
{
}

public class ProgressAchievementCommandHandler(IGamesDbContext context) : IRequestHandler<ProgressAchievementCommand>
{
    public async Task Handle(ProgressAchievementCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Achievements
            .FindAsync([request.AchievementId], cancellationToken);

        Guard.Against.NotFound(request.AchievementId, entity);

        Guard.Against.NotFound(request.GameId, entity.GameId);

        if (entity.ProgressCurrent < entity.ProgressTotal)
        {
            entity.ProgressCurrent = entity.ProgressCurrent + 1;
        }

        entity.UpdatedOn = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);

        if (entity.ProgressCurrent == entity.ProgressTotal)
        {
            // Add Event or Notification logic here for Achievement Completed
        }
    }
}
