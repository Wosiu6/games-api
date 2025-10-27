using Domain.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Games.Commands.CreateAchievement;

public record CreateAchievementCommand : IRequest<int>
{
    public int GameId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int Points { get; set; }
    public int ProgressTotal { get; set; }
    public int ProgressCurrent { get; set; }
}

public class CreateAchievementCommandHandler(IGamesDbContext context) : IRequestHandler<CreateAchievementCommand, int>
{
    public async Task<int> Handle(CreateAchievementCommand request, CancellationToken cancellationToken)
    {
        var achievement = new Achievement
        {
            GameId = request.GameId,
            Name = request.Name,
            Description = request.Description,
            Points = request.Points,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };

        context.Achievements.Add(achievement);
        await context.SaveChangesAsync(cancellationToken);

        return achievement.Id;
    }
}
