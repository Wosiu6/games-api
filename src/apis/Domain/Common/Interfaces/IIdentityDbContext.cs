using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Common.Interfaces;

public interface IIdentityDbContext
{
    DbSet<User>  Users { get; }
    DbSet<EmailVerificationToken>  EmailVerificationTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}