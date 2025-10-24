using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities;

[Table("Achievements")]
public class Achievement : BaseAuditableEntity
{
    public int GameId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int Points { get; set; }

    // navigation
    public Game? Game { get; set; }
}
