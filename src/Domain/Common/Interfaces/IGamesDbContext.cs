using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Common.Interfaces;

public interface IGamesDbContext
{
    DbSet<Game>  Games { get; }
    DbSet<Achievement> Achievements { get; }
    DbSet<UserGame> UserGames { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}