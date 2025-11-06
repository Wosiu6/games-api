using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities;

[Table("Games")]
public class Game : BaseAuditableEntity
{
    public string Title { get; set; } = null!;
    public DateTime ReleaseDate { get; set; }

    public string? Description { get; set; }
    public string? Genre { get; set; }
    public string? Developer { get; set; }
    public string? Publisher { get; set; }
    public decimal Price { get; set; }

    public ICollection<Achievement>? Achievements { get; set; } = new List<Achievement>();
    public ICollection<UserGame>? UserGames { get; set; } = new List<UserGame>();
}