using GamesApi.Domain.Interfaces;

namespace GamesApi.Domain.Common;

public class BaseEntity : IBaseEntity
{
    public int Id { get; set; }
}