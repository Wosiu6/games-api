using Application;
using Domain.Common.Data.Extensions;
using Infrastructure;
using Infrastructure.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddOpenApi();
builder.Services.AddOpenApiDocument(configuration, "Games");

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(configuration);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Scriptorium Api")
            .WithTheme(ScalarTheme.Purple)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });

    app.UseSwaggerUi();
    app.UseOpenApi();

    await app.InitialiseDatabaseAsync<GamesDbContextInitialiser>();
}

app.MapApplicationEndpoints();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();