using Domain.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class GamesDbContext(DbContextOptions<GamesDbContext> options) : DbContext(options), IGamesDbContext
{
    public DbSet<Game> Games { get; set; }
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<UserGame> UserGames { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("games");

        modelBuilder.Entity<UserGame>()
            .HasIndex(ug => new { ug.UserId, ug.GameId })
            .IsUnique(false);
    }
}