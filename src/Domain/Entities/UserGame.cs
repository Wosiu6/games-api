using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities;

[Table("UserGames")]
public class UserGame : BaseAuditableEntity
{
    public int UserId { get; set; }
    public int GameId { get; set; }
    public DateTime? OwnedAt { get; set; }
    public double PlayTimeHours { get; set; }
    public bool IsInstalled { get; set; }

    public User? User { get; set; }
    public Game? Game { get; set; }
}
