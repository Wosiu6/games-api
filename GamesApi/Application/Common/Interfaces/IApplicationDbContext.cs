using GamesApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GamesApi.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Game>  Games { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}