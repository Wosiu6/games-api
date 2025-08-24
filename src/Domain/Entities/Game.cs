using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities;

[Table("Games")]
public class Game : BaseAuditableEntity
{
    public string Title { get; set; }
    public DateTime ReleaseDate { get; set; }
}