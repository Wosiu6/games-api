using GamesApi.Application.Common.Interfaces;
using GamesApi.Domain.Entities;
using MediatR;

namespace GamesApi.Application.Games.Commands.CreateGame;

public abstract record CreateGameCommand : IRequest<int>
{
    public string Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    
}

public class CreateGameCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateGameCommand, int>
{
    public async Task<int> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        var entity = new Game
        {
            Title = request.Title,
            ReleaseDate = request.ReleaseDate,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };

        context.Games.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}