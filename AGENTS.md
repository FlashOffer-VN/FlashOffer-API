# FlashOffer — Cursor Cloud Agent

Hướng dẫn chạy project trên **Cursor Cloud Agent**. Đọc phần này trước khi start services.

## Repos trong multi-repo environment

Chọn **cả hai** repo khi tạo Cloud Agent environment:

| Repo | Vai trò |
|------|---------|
| `FlashOffer-API` | Backend .NET 9 + SQL Server (Docker) |
| `FlashOffer-UI` | Frontend Angular 20 |

File cấu hình chính nằm ở `FlashOffer-API/.cursor/environment.json`.

## Hai chế độ

### 1. Hybrid (mặc định) — khuyên dùng

- **Toolchain:** .NET 9 + Node 20 cài sẵn trên agent VM (build/test nhanh).
- **Docker:** chỉ cho API + SQL Server (`docker compose up`).
- **UI:** chạy native `npm start` (hot reload nhanh hơn container).

File: `.cursor/environment.json`

### 2. Toolchain only — task nhẹ

Dùng khi **không cần database** (sửa UI, unit test, lint, build).

File tham khảo: `.cursor/environment.toolchain-only.json`

> API sẽ lỗi khi gọi DB nếu không có SQL Server.

## Secrets (Cursor Dashboard → Cloud Agents → Secrets)

Thêm các biến sau (hoặc dùng giá trị mặc định trong `.env.docker.example` cho dev):

| Secret | Mục đích |
|--------|----------|
| `JWT_SECRET` | ≥ 32 ký tự |
| `SA_PASSWORD` | Mật khẩu SQL Server SA |
| `DB_CONNECTION_STRING` | (tùy chọn) override connection string |

## Khởi động full stack

Agent terminals tự chạy (xem `environment.json`):

1. **API stack** — `bash .cursor/start-api.sh`
   - SQL Server: `localhost:1433`
   - API: `http://localhost:5000`
   - Health: `http://localhost:5000/health`

2. **UI dev server** — `bash .cursor/start-ui.sh`
   - App: `http://localhost:4200`
   - API URL: chỉnh `src/environments/environment.ts` → `http://localhost:5000/api`

### Migration DB (lần đầu hoặc sau schema change)

```bash
cd FlashOffer-API
docker compose up -d sql-server
# đợi SQL healthy (~30s), rồi:
dotnet ef database update \
  --project src/FlashOffer.API.Infrastructure \
  --startup-project src/FlashOffer.API.WebApi \
  --connection "Server=localhost,1433;Database=FlashOffer.APIDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
```

Hoặc qua container API:

```bash
docker exec dotnet-api-base dotnet ef database update \
  --connection "Server=sql-server;Database=FlashOffer.APIDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
```

## Lệnh thường dùng (không cần Docker)

```bash
# API — build & test
cd FlashOffer-API && dotnet build && dotnet test

# UI — dev & test
cd FlashOffer-UI && npm ci --legacy-peer-deps && npm start
cd FlashOffer-UI && npm run test:ci && npm run build
```

## UI chạy trong Docker (tùy chọn)

```bash
export FLASHOFFER_UI_MODE=docker
bash .cursor/start-ui.sh
```

Hoặc trực tiếp trong repo UI: `docker compose up`.

## Port map

| Service | URL |
|---------|-----|
| Angular UI | http://localhost:4200 |
| API (Docker) | http://localhost:5000 |
| API (dotnet run) | https://localhost:7298 |
| SQL Server | localhost:1433 |

## Troubleshooting

| Vấn đề | Cách xử lý |
|--------|------------|
| `docker: command not found` | Dùng `environment.json` hybrid, không dùng snapshot cũ. Xóa snapshot cũ trên Dashboard nếu bị override. |
| API `/health` fail | `docker compose logs api sql-server`, đợi SQL healthy |
| UI không gọi được API | Kiểm tra `environment.ts` dùng `http://localhost:5000/api` khi API chạy Docker |
| DinD build fail | Thử `docker build --network=host ...` hoặc `docker compose build --build-arg BUILDKIT_INLINE_CACHE=1` |
| Không thấy FlashOffer-UI | Thêm repo vào multi-repo env hoặc set `FLASHOFFER_UI_ROOT=/path/to/FlashOffer-UI` |

## Tham chiếu

- [Cursor Cloud Environment Setup](https://cursor.com/docs/cloud-agent/setup)
- `docs/FlashOffer.API.Documentation/guides/docker-compose.md`
