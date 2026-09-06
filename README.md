# Rishta-Nata Management System

An ASP.NET Core MVC application for Nikah/marriage registration and certificate
management, backed by MySQL and authenticated against an external Tajneed member
API.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (`net10.0`)
- A MySQL server (database `rishtanatahdb`)

## Configuration

Local secrets/overrides live in a git-ignored `.env` file. To set up:

```bash
# Linux / macOS
cp .env.example .env

# Windows PowerShell
Copy-Item .env.example .env
```

Then edit `.env` and fill in real values — at minimum the MySQL connection
string:

```dotenv
ConnectionStrings__DefaultConnection=Server=localhost;Port=3306;Database=rishtanatahdb;User=root;Password=your_password;
```

The double underscore (`__`) maps to `:` in ASP.NET Core configuration, so the
line above overrides `ConnectionStrings:DefaultConnection` in
`appsettings.json`. Any other `appsettings.json` key can be overridden the same
way (e.g. `TajneedApiBaseUrl=...`, `RishtanataSecretary__ChandaNo=...`).

> `.env` is git-ignored. Never commit real credentials.

## Build & run

```bash
dotnet build AMJNRishtanata.slnx     # note: .slnx (XML solution format, no .sln)
dotnet run --project Presentation    # http://localhost:5032 / https://localhost:7246
```

The default route is the login page (`Auth/Login`).

## Project layout

| Project | Responsibility |
|---|---|
| `Domain/` | Entities, enums, domain events, constants |
| `Infrastructure/` | EF Core (MySQL), DTOs, mapper, migrations, seed |
| `Application/` | Services + interfaces (use-cases), authorization |
| `Gateway/` | HTTP client for the external Tajneed API |
| `Presentation/` | MVC controllers, Razor views, DI |

Authorization rules are documented in `docs/stage-authorization-policy.md`.
