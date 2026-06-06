```powershell
@"
# Docker Guide for FlashOffer.API

## Prerequisites

- Docker Desktop installed and running
- .NET 9.0 SDK (optional)

## Quick Start

### 1. Configure environment variables

```bash
cp .env.docker.example .env.docker
```

Edit `.env.docker` with your values.

### 2. Build and run

```bash
docker-compose up -d
```

### 3. Verify API is running

```bash
curl http://localhost:5000/health
```

## Docker Commands

| Command | Description |
|---------|-------------|
| `docker-compose up -d` | Start containers |
| `docker-compose down` | Stop containers |
| `docker-compose logs -f` | View logs |
| `docker-compose ps` | Container status |

## Configuration

### Environment Variables (.env.docker)

| Variable | Required | Description |
|----------|----------|-------------|
| DB_CONNECTION_STRING | âœ… | SQL Server connection |
| JWT_SECRET | âœ… | JWT secret (min 32 chars) |
| JWT_ISSUER | âŒ | JWT issuer |
| JWT_AUDIENCE | âŒ | JWT audience |
| LOG_LEVEL | âŒ | Log level |

### Ports

| Host | Container | Service |
|------|-----------|---------|
| 5000 | 80 | HTTP |
| 5001 | 443 | HTTPS |

## Troubleshooting

### Check logs

```bash
docker logs dotnet-api-base
```

### Health check

```bash
curl http://localhost:5000/health
```

## Security Notes

- Never commit `.env.docker` to Git
- Use strong JWT_SECRET (min 32 chars)
- Use secrets manager in production
"@ | Out-File -FilePath docker-guide.md -Encoding UTF8
```
