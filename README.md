Games API
==========

[![License][license-shield]][license-url] [![GitHub][github-shield]][github-url]

A .NET 9 Web API for managing games using Clean Architecture, CQRS, and PostgreSQL. It utilises JWT-based authentication with endpoint authorisation.

## Tech Stack

- **.NET 9** - Framework
- **Entity Framework Core** - RDBMS with PostgreSQL
- **MediatR** - CQRS implementation
- **AutoMapper** - Object mapping
- **Minimal APIs** - Endpoints
- Azure Identity package for JWT Authentication

## Prerequisites

- .NET 9 SDK
- PostgreSQL (if not using In-Memory DB)

## Setup
To run the application, you will need to either edit the Database setup to use an in-memory database or install the PostgreSQL server and create 2 databases, then replace the connection strings in applicationSettings.json
Data is pre-seeded in debug mode, so I'd recommend running in debug mode and grabbing a JWT token from the login endpoint, then adding the bearer token to your requests. You can do it in Swagger via the 'Authorize' button.
Alternatively, you can add a bearer to authentication in your header request in Postman/Bruon/Insomnia.

## Games API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/games` | Get all games |
| `POST` | `/games` | Create a game |
| `PUT` | `/games/{id}` | Update a game |
| `DELETE` | `/games/{id}` | Delete a game |

## Identity API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/users` | Registers a user |
| `POST` | `/users/login` | Returns a JWT token for authorization with the Games API |

## Running Migrations

### Games Api
- Initial creation: `dotnet ef migrations add InitialGamesCreate --verbose --project apis/Infrastructure/Infrastructure.csproj --startup-project apis/GamesApi/GamesApi.csproj --context ApplicationDbContext`
- Adding new migrations: `dotnet ef migrations add YourName --verbose --project apis/Infrastructure/Infrastructure.csproj --startup-project apis/GamesApi/GamesApi.csproj --context ApplicationDbContext`
- Updating db: `dotnet ef database update --verbose --project apis/Infrastructure/Infrastructure.csproj --startup-project apis/GamesApi/GamesApi.csproj --context ApplicationDbContext`
  
### Identity Api
- Initial creation: `dotnet ef migrations add InitialIdentityCreate --verbose --project apis/Infrastructure/Infrastructure.csproj --startup-project apis/IdentityApi/IdentityApi.csproj --context IdentityDbContext`
- Adding new migrations: `dotnet ef migrations add YourName --verbose --project apis/Infrastructure/Infrastructure.csproj --startup-project apis/IdentityApi/IdentityApi.csproj --context IdentityDbContext`
- Updating db: `dotnet ef database update --verbose --project apis/Infrastructure/Infrastructure.csproj --startup-project apis/IdentityApi/IdentityApi.csproj --context IdentityDbContext`

## TODO
- API Gateway
- Balance Loader
- Host on Azure
- Azure Flexible Database + PostgreSQL

[paypal-shield]: https://img.shields.io/static/v1?label=PayPal&message=Donate&style=flat-square&logo=paypal&color=blue
[paypal-url]: https://www.paypal.com/donate/?hosted_button_id=MTY5DP7G8G6T4

[coffee-shield]: https://img.shields.io/static/v1?label=BuyMeCoffee&message=Donate&style=flat-square&logo=buy-me-a-coffee&color=orange
[coffee-url]: https://www.buymeacoffee.com/wosiu6

[license-shield]: https://img.shields.io/badge/license-Apache%20License%202.0-purple
[license-url]: https://opensource.org/license/apache-2-0

[github-shield]: https://img.shields.io/static/v1?label=&message=GitHub&style=flat-square&logo=github&color=grey
[github-url]: https://github.com/Wosiu6/games-api

