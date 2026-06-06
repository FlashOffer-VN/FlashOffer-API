# Docker Compose

## Má»¥c Ä‘Ã­ch

TÃ i liá»‡u nÃ y hÆ°á»›ng dáº«n cÃ¡ch cháº¡y API vÃ  SQL Server báº±ng Docker Compose cho mÃ´i trÆ°á»ng local.

## File liÃªn quan

- `docker-compose.yml`
- `.env.docker.example`
- `Dockerfile`
- `README.md`
- `docs/FlashOffer.API.Documentation/env-vars.md`

## Cáº¥u trÃºc docker-compose

- `api`: chá»©a dá»‹ch vá»¥ `dotnet-api-base`, build tá»« `Dockerfile`.
- `sql-server`: container SQL Server 2022.
- volumes: lÆ°u dá»¯ liá»‡u SQL vÃ o `mssql-data`.

## Cháº¡y Docker Compose

1. Copy máº«u:

```bash
cp .env.docker.example .env.docker
```

2. Chá»‰nh `.env.docker` theo mÃ´i trÆ°á»ng cá»§a báº¡n.

3. Cháº¡y Docker Compose:

```bash
docker-compose up -d
```

4. Kiá»ƒm tra tráº¡ng thÃ¡i:

```bash
docker-compose ps
docker-compose logs -f api
```

5. Kiá»ƒm tra API:

```bash
curl http://localhost:5000/health
```

## Biáº¿n mÃ´i trÆ°á»ng quan trá»ng

- `DB_CONNECTION_STRING`
- `JWT_SECRET`
- `JWT_ISSUER`
- `JWT_AUDIENCE`
- `JWT_EXPIRY_MINUTES`
- `LOG_LEVEL`
- `SA_PASSWORD`
- `ACCEPT_EULA`

## Ghi chÃº

- `docker-compose.yml` hiá»‡n cáº¥u hÃ¬nh cá»•ng host `5000` â†’ container `80`, vÃ  `5001` â†’ container `443`.
- SQL Server dÃ¹ng `sql-server` lÃ m tÃªn mÃ¡y chá»§ ná»™i bá»™.
- Náº¿u cáº§n cháº¡y migration sau khi container cháº¡y, dÃ¹ng:

```bash
docker exec dotnet-api-base dotnet ef database update --connection "Server=sql-server;Database=FlashOffer.APIDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
```
