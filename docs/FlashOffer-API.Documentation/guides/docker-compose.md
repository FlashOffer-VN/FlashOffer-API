# Docker Compose

## Mục đích

Tài liệu này hướng dẫn cách chạy API và SQL Server bằng Docker Compose cho môi trường local.

## File liên quan

- `docker-compose.yml`
- `.env.docker.example`
- `Dockerfile`
- `README.md`
- `docs/FlashOffer-API.Documentation/env-vars.md`

## Cấu trúc docker-compose

- `api`: chứa dịch vụ `dotnet-api-base`, build từ `Dockerfile`.
- `sql-server`: container SQL Server 2022.
- volumes: lưu dữ liệu SQL vào `mssql-data`.

## Chạy Docker Compose

1. Copy mẫu:

```bash
cp .env.docker.example .env.docker
```

2. Chỉnh `.env.docker` theo môi trường của bạn.

3. Chạy Docker Compose:

```bash
docker-compose up -d
```

4. Kiểm tra trạng thái:

```bash
docker-compose ps
docker-compose logs -f api
```

5. Kiểm tra API:

```bash
curl http://localhost:5000/health
```

## Biến môi trường quan trọng

- `DB_CONNECTION_STRING`
- `JWT_SECRET`
- `JWT_ISSUER`
- `JWT_AUDIENCE`
- `JWT_EXPIRY_MINUTES`
- `LOG_LEVEL`
- `SA_PASSWORD`
- `ACCEPT_EULA`

## Ghi chú

- `docker-compose.yml` hiện cấu hình cổng host `5000` → container `80`, và `5001` → container `443`.
- SQL Server dùng `sql-server` làm tên máy chủ nội bộ.
- Nếu cần chạy migration sau khi container chạy, dùng:

```bash
docker exec dotnet-api-base dotnet ef database update --connection "Server=sql-server;Database=FlashOffer-APIDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
```
