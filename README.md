# Games API

A .NET 9 Web API for managing games using Clean Architecture, CQRS, and PostgreSQL.

## Tech Stack

- **.NET 9** - Framework
- **Entity Framework Core** - RDBMS with PostgreSQL
- **MediatR** - CQRS implementation
- **AutoMapper** - Object mapping
- **Minimal APIs** - Endpoints

## Prerequisites

- .NET 9 SDK
- PostgreSQL

## Setup
To run the application, you will need to either edit the Database setup to use an in-memory database or install the PostgreSQL server and create 2 databases, then replace the connection strings in applicationSettings.json

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
- User API
- Authentication
- Authorization
- API Gateway
- Balance Loader
- Host on Azure
- Azure Flexible Database + PostgreSQL
