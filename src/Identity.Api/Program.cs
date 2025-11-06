using Application;
using Domain.Common.Data.Extensions;
using HealthChecks.UI.Client;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi();
builder.Services.AddOpenApiDocument(configuration, "Identity");

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(configuration);

builder.Services.AddIdentityApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Identity Api")
            .WithTheme(ScalarTheme.Purple)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });

    app.UseSwaggerUi();
    app.UseOpenApi();

    await app.InitialiseDatabaseAsync<IdentityDbContextInitialiser>();
}

app.UseHttpsRedirection();

app.MapIdentityEndpoints();

app.Run();