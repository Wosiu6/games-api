using Domain.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Games.Commands.CreateGame;

public record CreateGameCommand : IRequest<int>
{
    public string Title { get; set; } = null!;
    public DateTime ReleaseDate { get; set; }
    public string? Description { get; set; }
    public string? Genre { get; set; }
    public string? Developer { get; set; }
    public string? Publisher { get; set; }
    public decimal? Price { get; set; }
}

public class CreateGameCommandHandler(IGamesDbContext context) : IRequestHandler<CreateGameCommand, int>
{
    public async Task<int> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        var entity = new Game
        {
            Title = request.Title,
            ReleaseDate = request.ReleaseDate,
            Description = request.Description,
            Genre = request.Genre,
            Developer = request.Developer,
            Publisher = request.Publisher,
            Price = request.Price ?? 0m,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };

        context.Games.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
