using Application.Identity.Commands.CreateUser;
using AutoMapper.QueryableExtensions;
using Domain.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Identity.Queries.GetUsers;

public record GetUsersQuery() : IRequest<UsersVm>;

public class GetUsersQueryHandler(IIdentityDbContext context, AutoMapper.IMapper mapper) : IRequestHandler<GetUsersQuery, UsersVm>
{
    public async Task<UsersVm> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var items = await context.Users
            .ProjectTo<UserDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new UsersVm { Items = items };
    }
}

public class UsersVm
{
    public List<UserDto> Items { get; set; } = [];
}
