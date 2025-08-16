using GamesApi.Application.Common.Interfaces;
using GamesApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GamesApi.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<Game> Games { get; set; }
}