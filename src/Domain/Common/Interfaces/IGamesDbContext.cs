using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Common.Interfaces;

public interface IGamesDbContext
{
    DbSet<Game>  Games { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}