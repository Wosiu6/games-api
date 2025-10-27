using Ardalis.GuardClauses;
using Domain.Common.Interfaces;
using MediatR;

namespace Application.Games.Commands.UpdateGame;

public record ProgressAchievementCommand(int Id) : IRequest
{
    public string AchievementId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int Points { get; set; }
    public int ProgressTotal { get; set; }
    public int ProgressCurrent { get; set; }
}

public class ProgressAchievementCommandHandler(IGamesDbContext context) : IRequestHandler<ProgressAchievementCommand>
{
    public async Task Handle(ProgressAchievementCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Achievements
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        if (entity.ProgressCurrent < entity.ProgressTotal)
        {
            entity.ProgressCurrent = request.ProgressCurrent + 1;
        }

        entity.UpdatedOn = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);

        if (entity.ProgressCurrent == entity.ProgressTotal)
        {
            // Add Event or Notification logic here for Achievement Completed
        }
    }
}
