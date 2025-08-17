using Ardalis.GuardClauses;
using Domain.Common.Interfaces;
using MediatR;

namespace Games.Games.Commands.UpdateGame;

public record UpdateGameCommand(int Id) : IRequest
{
    public string Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    
}

public class UpdateGameCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateGameCommand>
{
    public async Task Handle(UpdateGameCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Games
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Title = request.Title;
        entity.ReleaseDate = request.ReleaseDate;
        entity.UpdatedOn = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
    }
}