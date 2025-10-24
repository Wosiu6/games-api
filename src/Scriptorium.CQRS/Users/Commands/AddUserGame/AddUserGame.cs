using Domain.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Scriptorium.CQRS.Users.Commands.AddUserGame;

public record AddUserGameCommand : IRequest<int>
{
    public int UserId { get; set; }
    public int GameId { get; set; }
    public DateTime? OwnedAt { get; set; }
    public double PlayTimeHours { get; set; }
    public bool IsInstalled { get; set; }
}

public class AddUserGameCommandHandler(IGamesDbContext context) : IRequestHandler<AddUserGameCommand, int>
{
    public async Task<int> Handle(AddUserGameCommand request, CancellationToken cancellationToken)
    {
        var entity = new UserGame
        {
            UserId = request.UserId,
            GameId = request.GameId,
            OwnedAt = request.OwnedAt ?? DateTime.UtcNow,
            PlayTimeHours = request.PlayTimeHours,
            IsInstalled = request.IsInstalled,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };

        context.UserGames.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
