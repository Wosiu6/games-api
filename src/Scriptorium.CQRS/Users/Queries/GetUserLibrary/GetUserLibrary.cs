using AutoMapper.QueryableExtensions;
using Domain.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Scriptorium.CQRS.Users.Queries.GetUserLibrary;

public record GetUserLibraryQuery(int UserId) : IRequest<UserLibraryVm>;

public class GetUserLibraryQueryHandler(IGamesDbContext context, AutoMapper.IMapper mapper) : IRequestHandler<GetUserLibraryQuery, UserLibraryVm>
{
    public async Task<UserLibraryVm> Handle(GetUserLibraryQuery request, CancellationToken cancellationToken)
    {
        var items = await context.UserGames
            .Where(ug => ug.UserId == request.UserId)
            .ProjectTo<UserGameDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new UserLibraryVm { Items = items };
    }
}

public class UserLibraryVm
{
    public List<UserGameDto> Items { get; set; } = new();
}
