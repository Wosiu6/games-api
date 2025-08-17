using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Game>  Games { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}