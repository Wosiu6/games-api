using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace UsersApi.Extensions;

public static class MigrationsExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateAsyncScope();
        
        using IdentityDbContext ctx = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
        
        ctx.Database.Migrate();
    }
}