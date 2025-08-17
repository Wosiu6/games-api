using System.Text;
using Application;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Data.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();

builder.Services.AddAuthorization();

builder.Services.AddApplicationServices();

builder.Services.ConfigureGamesDatabaseContext(builder.Configuration);

builder.Services.AddScoped<ApplicationDbContextInitialiser>();

builder.Services.ConfigureGamesMediatR();

builder.Services.AddOpenApiDocument();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseOpenApi();
    await app.InitialiseDatabaseAsync<ApplicationDbContextInitialiser>();
}

app.MapGamesEndpoints();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();