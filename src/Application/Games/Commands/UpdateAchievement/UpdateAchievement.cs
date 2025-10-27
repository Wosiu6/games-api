using Ardalis.GuardClauses;
using Domain.Common.Interfaces;
using MediatR;

namespace Application.Games.Commands.UpdateGame;

public record UpdateAchievementCommand(int Id) : IRequest
{
    public string AchievementId { get; set; } = null!;
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
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Points = request.Points;
        entity.ProgressTotal = request.ProgressTotal;
        entity.ProgressCurrent = request.ProgressCurrent;
        entity.UpdatedOn = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
    }
}
