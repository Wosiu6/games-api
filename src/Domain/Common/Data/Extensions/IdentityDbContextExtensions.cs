using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Common.Data.Extensions
{
    public static class IdentityDbContextExtensions
    {
        public static async Task<User?> GetByEmailAsync(this DbSet<User> users, string email, CancellationToken cancellationToken)
        {
            return await users.SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
        }
    }
}
