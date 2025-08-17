using Domain.Common.Interfaces;

namespace Domain.Common;

public class BaseAuditableEntity : IBaseEntity, IAuditableEntity
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
}