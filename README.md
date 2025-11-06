Games API
==========

[![License][license-shield]][license-url] [![GitHub][github-shield]][github-url]

A .NET 9 Web API for managing games and user libraries using **Clean Architecture**, **CQRS**, and **PostgreSQL**. Features **JWT-based authentication** with fine-grained endpoint authorization.

The solution is structured into two APIs:

- **Games API** - Core game management (CRUD, achievements, progress), User-specific game library management
- **Identity API** - User registration and authentication

---

## Tech Stack

- **.NET 9** - Framework
- **Entity Framework Core** - ORM with PostgreSQL
- **MediatR** - CQRS pattern implementation
- **AutoMapper** - Object-to-object mapping
- **Minimal APIs** - Lightweight endpoint definitions
- **Azure Identity** - JWT authentication & authorization

---

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- PostgreSQL (or use In-Memory DB for testing)

---

## Setup

1. **Database Configuration**  
   Edit `applicationSettings.json` in each API project to use either:
   - **In-Memory DB** (for quick testing)
   - **PostgreSQL** - create two databases:
     - `GamesDb`
     - `IdentityDb`

2. **Run Migrations** (see below)

3. **Seed Data & Authentication**  
   Run in **Debug mode** to auto-seed initial data.  
   Use the `/users/login` endpoint to get a **JWT token**, then:
   - In **Swagger**: Click **Authorize** → paste `Bearer <token>`
   - In **Postman/Insomnia**: Add `Authorization: Bearer <token>` header

---

### Games API

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET`    | `/games` | Get all games |
| `POST`   | `/games` | Create a new game |
| `PUT`    | `/games/{id}` | Update a game |
| `DELETE` | `/games/{id}` | Delete a game |
| `GET`    | `/games/{id}/achievements` | Get achievements for a game |
| `POST`   | `/games/{id}/achievements` | Create achievement |
| `PUT`    | `/achievements/{id}` | Update achievement |
| `POST`   | `/achievements/{id}/progress` | Progress user achievement |
| `POST` | `/users/games` | Add game to user library |
| `GET`  | `/users/library` | Get user's game library |

### Identity API

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/users` | Register a new user |
| `POST` | `/users/login` | Login and receive JWT token |

---

## Running Migrations

### Games API (`ApplicationDbContext`)

```bash
# Initial migration
dotnet ef migrations add InitialGamesCreate \
  --project apis/Infrastructure/Infrastructure.csproj \
  --startup-project apis/GamesApi/GamesApi.csproj \
  --context ApplicationDbContext \
  --verbose

# Add new migration
dotnet ef migrations add YourMigrationName \
  --project apis/Infrastructure/Infrastructure.csproj \
  --startup-project apis/GamesApi/GamesApi.csproj \
  --context ApplicationDbContext \
  --verbose

# Apply to database
dotnet ef database update \
  --project apis/Infrastructure/Infrastructure.csproj \
  --startup-project apis/GamesApi/GamesApi.csproj \
  --context ApplicationDbContext \
  --verbose
```

### Identity API (`IdentityDbContext`)

```bash
# Initial migration
dotnet ef migrations add InitialIdentityCreate \
  --project apis/Infrastructure/Infrastructure.csproj \
  --startup-project apis/IdentityApi/IdentityApi.csproj \
  --context IdentityDbContext \
  --verbose

# Add new migration
dotnet ef migrations add YourMigrationName \
  --project apis/Infrastructure/Infrastructure.csproj \
  --startup-project apis/IdentityApi/IdentityApi.csproj \
  --context IdentityDbContext \
  --verbose

# Apply to database
dotnet ef database update \
  --project apis/Infrastructure/Infrastructure.csproj \
  --startup-project apis/IdentityApi/IdentityApi.csproj \
  --context IdentityDbContext \
  --verbose
```

---

## Authentication & Authorization

- JWT tokens issued on login (`/users/login`)
- Protected endpoints require valid `Bearer` token
- Role/claim-based authorization applied via policies
- Swagger UI supports token input via **Authorize** button

---

## TODO

- [x] Implement **API Gateway**
- [x] Docker compose and Dockermake files
- [ ] Add **Load Balancer** for scalability
- [ ] Use **Azure Flexible Database for PostgreSQL**
- [ ] Add **Rate Limiting** and **Caching**, possibly Redis

---

## License

MIT

---

[paypal-shield]: https://img.shields.io/static/v1?label=PayPal&message=Donate&style=flat-square&logo=paypal&color=blue
[paypal-url]: https://www.paypal.com/donate/?hosted_button_id=MTY5DP7G8G6T4

[coffee-shield]: https://img.shields.io/static/v1?label=BuyMeCoffee&message=Donate&style=flat-square&logo=buy-me-a-coffee&color=orange
[coffee-url]: https://www.buymeacoffee.com/wosiu6

[license-shield]: https://img.shields.io/badge/license-MITLicense%202.0-purple
[license-url]: https://opensource.org/license/mit

[github-shield]: https://img.shields.io/static/v1?label=&message=GitHub&style=flat-square&logo=github&color=grey
[github-url]: https://github.com/Wosiu6/games-api
