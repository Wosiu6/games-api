using System.ComponentModel.DataAnnotations.Schema;
using GamesApi.Domain.Common;

namespace GamesApi.Domain.Entities;

[Table("Games")]
public class Game : BaseAuditableEntity
{
    public string Title { get; set; }
    public DateTime ReleaseDate { get; set; }
}