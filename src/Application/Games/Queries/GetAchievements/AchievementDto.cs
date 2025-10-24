using AutoMapper;
using Domain.Entities;

namespace Application.Games.Queries.GetAchievements;

public class AchievementDto
{
    public int Id { get; init; }
    public int GameId { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public int Points { get; init; }

    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Achievement, AchievementDto>();
        }
    }
}
