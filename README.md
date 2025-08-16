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

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/games` | Get all games |
| `POST` | `/games` | Create a game |
| `PUT` | `/games{id}` | Update a game |
| `DELETE` | `/games{id}` | Delete a game |

## TODO
- User API
- Authentication
- Authorization
- API Gateway
- Balance Loader
- Host on Azure
- Azure Flexible Database + PostgreSQL
