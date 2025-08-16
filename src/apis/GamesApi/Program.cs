using GamesApi.Application;
using GamesApi.Application.Games.Commands.CreateGame;
using GamesApi.Infrastructure;
using GamesApi.Infrastructure.Data;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", true,true)
    .Build();

builder.Services.AddOpenApi();

builder.Services.ConfigureDatabaseContext(configuration);
builder.Services.AddScoped<ApplicationDbContextInitialiser>();

builder.Services.ConfigureMediatR();
builder.Services.AddOpenApiDocument();
builder.Services.AddApplicationServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseOpenApi();
    app.UseSwaggerUi();
    await app.InitialiseDatabaseAsync();
}

app.MapEndpoints();

app.UseHttpsRedirection();
app.Run();