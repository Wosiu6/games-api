using Ardalis.GuardClauses;
using Domain.Common.Interfaces;
using MediatR;

namespace Application.Games.Commands.UpdateGame;

public record UpdateGameCommand(int Id) : IRequest
{
    public string Title { get; set; } = null!;
    public DateTime ReleaseDate { get; set; }
    public string? Description { get; set; }
    public string? Genre { get; set; }
    public string? Developer { get; set; }
    public string? Publisher { get; set; }
    public decimal? Price { get; set; }
    
}

public class UpdateGameCommandHandler(IGamesDbContext context) : IRequestHandler<UpdateGameCommand>
{
    public async Task Handle(UpdateGameCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Games
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Title = request.Title;
        entity.ReleaseDate = request.ReleaseDate;
        entity.Description = request.Description;
        entity.Genre = request.Genre;
        entity.Developer = request.Developer;
        entity.Publisher = request.Publisher;
        entity.Price = request.Price ?? entity.Price;
        entity.UpdatedOn = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
    }
}
