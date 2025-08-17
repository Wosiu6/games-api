namespace Domain.Common.Interfaces;

public interface IAuditableEntity
{
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
}