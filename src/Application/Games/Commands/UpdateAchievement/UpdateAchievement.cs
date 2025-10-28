using Ardalis.GuardClauses;
using Domain.Common.Interfaces;
using MediatR;

namespace Application.Games.Commands.UpdateAchievement;

public record UpdateAchievementCommand(int AchievementId, int GameId) : IRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int Points { get; set; }
    public int ProgressTotal { get; set; }
    public int ProgressCurrent { get; set; }
}

public class UpdateAchievementCommandHandler(IGamesDbContext context) : IRequestHandler<UpdateAchievementCommand>
{
    public async Task Handle(UpdateAchievementCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Achievements
            .FindAsync([request.AchievementId], cancellationToken);

        Guard.Against.NotFound(request.AchievementId, entity);
        Guard.Against.NotFound(request.GameId, entity.GameId);

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Points = request.Points;
        entity.ProgressTotal = request.ProgressTotal;
        entity.ProgressCurrent = request.ProgressCurrent;
        entity.UpdatedOn = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
    }
}
