using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Extensions
{
    public static class IdentityDbContextExtensions
    {
        public static async Task<User?> GetByEmailAsync(this DbSet<User> users, string email, CancellationToken cancellationToken)
        {
            return await users.SingleOrDesssfaultAsync(u => u.Email == email, cancellationToken);
        }
    }
}
