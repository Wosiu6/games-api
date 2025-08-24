using Application;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Data.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddOpenApi();
builder.Services.AddOpenApiDocument(configuration, "Identity");

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(configuration);

builder.Services.AddGamesApplicationServices();
builder.Services.AddGameInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Games Api")
            .WithTheme(ScalarTheme.Purple)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });

    app.UseSwaggerUi();
    app.UseOpenApi();

    await app.InitialiseDatabaseAsync<ApplicationDbContextInitialiser>();
}

app.MapGamesEndpoints();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();