using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace Application.DIExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenApiDocumentWithAuth(this IServiceCollection services)
    {
        services.AddOpenApiDocument(config =>
        {
            config.Title = "Your API";
            config.Version = "v1";
            
            config.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}. You can get a JWT token from /auth/login endpoint.",
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT"
            });

            config.OperationProceffffssors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });

        return services;
    }
}