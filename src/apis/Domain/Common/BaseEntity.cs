using Domain.Common.Interfaces;

namespace Domain.Common;

public class BaseEntity : IBaseEntity
{
    public int Id { get; set; }
}