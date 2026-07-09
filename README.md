# FlashOffer.API

FlashOffer.API is a .NET 10 Web API project structured with a Clean Architecture approach. Developer documentation and contribution guidelines are kept under docs/FlashOffer.API.Documentation/.

Table of contents
- Features
- Requirements
- Quick start
- Project structure
- Contributing

Key technologies

| Technology | Version |
|---|---:|
| .NET SDK | 10.0 |
| Entity Framework Core | 10.0 |
| ASP.NET Core Web API | 10.0 |
| AutoMapper | 12.x |
| FluentValidation | 11.x |
| Microsoft.IdentityModel.Tokens / JwtBearer | 8.x |
| xUnit | 2.6.2 |
| Serilog | 8.x |
| Swagger/Swashbuckle | 6.5.x |

Before you start

- Install .NET 10 SDK: https://dotnet.microsoft.com/download/dotnet/10.0
- Install Git and clone the repository

Quick start

1. Clone the repository

```bash
git clone https://github.com/FlashOffer-VN/FlashOffer-API.git
cd FlashOffer-API
```

2. Restore and build

```bash
dotnet restore
dotnet build
```

3. Run tests

```bash
dotnet test
```

4. Run the API

```bash
cd src/FlashOffer.API.WebApi
dotnet run
```

Deploy to Render

1. Push branch `longdt6` to GitHub.
2. On [Render](https://render.com), create a **PostgreSQL 16** database (e.g. `flashoffer-db`) and note the **Internal Database URL**.
3. Create a **Web Service** from the repo: **Runtime = Docker**, **Branch = longdt6**, **Health Check Path = `/health`**.
4. Set environment variables on the Web Service:

| Key | Value |
|-----|-------|
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `DB_CONNECTION_STRING` | Internal Database URL (or link the Postgres instance) |
| `JWT_SECRET` | Random string, at least 32 characters |
| `JWT_ISSUER` | `FlashOffer.API` |
| `JWT_AUDIENCE` | `FlashOffer.APIClient` |
| `JWT_EXPIRY_MINUTES` | `60` |
| `LOG_LEVEL` | `Information` |
| `ALLOWED_ORIGINS` | `http://localhost:3000,http://localhost:4200,https://flash-offer-ui.vercel.app,https://www.kindi.vn,https://kindi.vn` |

5. Deploy and verify `https://<your-service>.onrender.com/health` returns `Healthy`.
6. Log in with the seeded admin account: username `admin`, password `Admin@123`.

Optional: set `ENABLE_SWAGGER=true` to expose Swagger UI in Production.

Developer docs

See docs/FlashOffer.API.Documentation/ for detailed developer guidance, contributing rules, coding conventions, and API templates.

Contributing

Before opening a PR:
- Run `dotnet build` and `dotnet test` and ensure tests pass.
- Follow the detailed guide in `docs/FlashOffer.API.Documentation/CONTRIBUTING.md`.
- If you change package versions, update `Directory.Packages.props` and document the reason in the PR.

License

Internal use only.