using Ardalis.GuardClauses;
using Domain.Common.Interfaces;
using MediatR;

namespace Application.Games.Commands.DeleteGame;

public record DeleteGameCommand(int Id) : IRequest;

public class DeleteGameCommandHandler(IGamesDbContext context) : IRequestHandler<DeleteGameCommand>
{
    public async Task Handle(DeleteGameCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Games
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.Games.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }

}
