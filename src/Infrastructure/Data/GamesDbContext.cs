using Domain.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class GamesDbContext(DbContextOptions<GamesDbContext> options) : DbContext(options), IGamesDbContext
{
    public DbSet<Game> Games { get; set; }
}