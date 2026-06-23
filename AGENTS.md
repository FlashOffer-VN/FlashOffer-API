# AGENTS.md

## Cursor Cloud specific instructions

FlashOffer.API is a .NET 9 (Clean Architecture) Web API for a lead-capture / group-buy product. It persists to **SQL Server**. Standard build/test/run commands live in `README.md`; the notes below cover only the non-obvious, environment-specific gotchas.

### Toolchain
- .NET 9 SDK is installed at `/usr/local/dotnet` (symlinked to `/usr/local/bin/dotnet`). `DOTNET_ROOT` and `~/.dotnet/tools` are exported from `~/.bashrc` (needed for the `dotnet-ef` global tool).

### SQL Server (required for running the API; not for tests)
- SQL Server 2022 is installed natively. This container has **no systemd**, so `systemctl` does not work — start the engine manually (e.g. in a background tmux session):
  - `sudo -u mssql /opt/mssql/bin/sqlservr`
- It listens on `localhost:1433`. Credentials: user `sa`, password `YourStrong!Passw0rd`. Query with `/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'YourStrong!Passw0rd' -C ...`.

### Database schema / migrations (important)
- The repo ships **no EF migrations**. An `InitialCreate` migration was generated locally under `src/FlashOffer.API.Infrastructure/Migrations/` (untracked) and applied to `FlashOffer.APIDb`.
- `DesignTimeDbContextFactory` points at Windows LocalDB, so design-time DB operations on Linux must pass `--connection`. To (re)create the schema:
  - `dotnet-ef migrations add InitialCreate --project src/FlashOffer.API.Infrastructure --startup-project src/FlashOffer.API.WebApi` (only if the `Migrations/` folder is missing)
  - `dotnet-ef database update --project src/FlashOffer.API.Infrastructure --startup-project src/FlashOffer.API.WebApi --connection "Server=localhost,1433;Database=FlashOffer.APIDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"`
- The app does **not** auto-create or migrate the DB on startup, so the schema must exist before running.

### Configuration
- `Program.cs` loads a `.env` from the repo root (keys: `DB_CONNECTION_STRING`, `JWT_*`, `ALLOWED_ORIGINS`). A local `.env` exists (untracked) pointing `DB_CONNECTION_STRING` at the local SQL Server. Without `.env`, it falls back to the LocalDB connection string in `appsettings.json`, which does not work on Linux.

### Running the API (development)
- From `src/FlashOffer.API.WebApi`: `ASPNETCORE_ENVIRONMENT=Development ASPNETCORE_URLS=http://localhost:5289 dotnet run --no-launch-profile`
- Swagger UI: `http://localhost:5289/swagger` · Health: `http://localhost:5289/health` · Lead capture: `POST http://localhost:5289/api/leads/offer-requests`.
- The default launch profile uses HTTPS `7298`; the plain HTTP `5289` profile above avoids dev-cert hassle. The Angular UI's dev `environment.ts` expects `https://localhost:7298/api`.

### Tests
- `dotnet test` works without SQL Server (integration tests use EF InMemory).
