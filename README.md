# FlashOffer.API

FlashOffer.API is a .NET 9 Web API project structured with a Clean Architecture approach. Developer documentation and contribution guidelines are kept under docs/FlashOffer.API.Documentation/.

Table of contents
- Features
- Requirements
- Quick start
- Project structure
- Contributing

Key technologies

| Technology | Version |
|---|---:|
| .NET SDK | 9.0 |
| Entity Framework Core | 9.0 |
| ASP.NET Core Web API | 9.0 |
| AutoMapper | 12.x |
| FluentValidation | 11.x |
| Microsoft.IdentityModel.Tokens / JwtBearer | 8.x |
| xUnit | 2.6.2 |
| Serilog | 8.x |
| Swagger/Swashbuckle | 6.5.x |

Before you start

- Install .NET 9 SDK: https://dotnet.microsoft.com/download/dotnet/9.0
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

Developer docs

See docs/FlashOffer.API.Documentation/ for detailed developer guidance, contributing rules, coding conventions, and API templates.

Contributing

Before opening a PR:
- Run `dotnet build` and `dotnet test` and ensure tests pass.
- Follow the detailed guide in `docs/FlashOffer.API.Documentation/CONTRIBUTING.md`.
- If you change package versions, update `Directory.Packages.props` and document the reason in the PR.

License

Internal use only.