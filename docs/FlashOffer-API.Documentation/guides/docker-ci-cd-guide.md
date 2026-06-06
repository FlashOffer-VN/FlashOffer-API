```markdown
# Hướng dẫn cấu hình Docker và CI/CD

## Docker

### 1. Cấu hình môi trường

Tạo file `.env.docker` từ mẫu `.env.example`:

```bash
cp .env.example .env.docker
```

Sửa các biến môi trường trong `.env.docker`:

```env
DB_CONNECTION_STRING=Server=sql-server;Database={ProjectName}Db;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;
JWT_SECRET=YourSuperSecretKeyHereAtLeast32CharactersLong!
JWT_ISSUER={ProjectName}
JWT_AUDIENCE={ProjectName}Client
JWT_EXPIRY_MINUTES=60
LOG_LEVEL=Information
```

### 2. Chạy với Docker Compose

```bash
docker-compose up -d
```

### 3. Kiểm tra container

```bash
docker-compose ps
docker-compose logs -f api
```

### 4. Dừng container

```bash
docker-compose down
```

### 5. Xóa toàn bộ (bao gồm volumes)

```bash
docker-compose down -v
```

## CI/CD với GitHub Actions

### 1. Các secrets cần cấu hình

Vào **GitHub Repository → Settings → Secrets and variables → Actions**:

| Secret name | Mô tả | Bắt buộc |
|-------------|-------|----------|
| `DOCKER_USERNAME` | Tên đăng nhập Docker Hub | ✅ Có (nếu dùng Docker Hub) |
| `DOCKER_TOKEN` | Personal Access Token Docker Hub | ✅ Có (nếu dùng Docker Hub) |
| `AZURE_CREDENTIALS` | Azure service principal JSON | ❌ Nếu deploy lên Azure |
| `AZURE_WEBAPP_NAME` | Tên Azure Web App | ❌ Nếu deploy lên Azure |

### 2. Workflows có sẵn

| Workflow | File | Khi nào chạy |
|----------|------|--------------|
| CI Build & Test | `ci.yml` | Push lên master, Pull Request |
| Docker Build & Push | `docker.yml` | Push lên master, tạo tag |

### 3. Tùy chỉnh CI/CD

#### Chỉ chạy CI (không Docker)

Xóa hoặc comment file `docker.yml`:

```bash
rm .github/workflows/docker.yml
```

#### Thêm môi trường staging

Sao chép `ci.yml` thành `ci-staging.yml` và sửa branch thành `staging`.

#### Deploy lên Azure Web App

Thêm workflow `.github/workflows/azure-deploy.yml`:

```yaml
name: Deploy to Azure

on:
  push:
    branches: [ master ]

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: Deploy to Azure Web App
        uses: azure/webapps-deploy@v2
        with:
          app-name: ${{ secrets.AZURE_WEBAPP_NAME }}
          publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
          package: .
```

## Kiểm tra kết nối database trong Docker

```bash
# Kiểm tra SQL Server
docker exec -it dotnet-sql-server /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P YourStrong!Passw0rd -C -Q "SELECT 1"

# Kiểm tra API có kết nối database không
curl http://localhost:5000/api/v1/sample
```

## Xem logs

```bash
# Logs của API
docker logs dotnet-api-base -f

# Logs của SQL Server
docker logs dotnet-sql-server -f

# Logs của tất cả container
docker-compose logs -f
```

## Troubleshooting

### Lỗi: "LocalDB is not supported on this platform"

**Nguyên nhân:** LocalDB không hoạt động trong Linux container.

**Giải pháp:** Dùng SQL Server container như trong `docker-compose.yml`.

### Lỗi: "Port already in use"

```bash
# Kiểm tra port đang dùng
netstat -an | findstr "5000"
netstat -an | findstr "1433"

# Đổi port trong docker-compose.yml
ports:
  - "5001:80"  # Thay 5000 bằng port khác
```

### Lỗi: "Cannot connect to database"

```bash
# Kiểm tra SQL Server đã sẵn sàng chưa
docker logs dotnet-sql-server

# Chạy migration thủ công
docker exec dotnet-api-base dotnet ef database update --connection "Server=sql-server;Database={ProjectName}Db;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
```

## Tài liệu tham khảo

- [Docker Compose file reference](https://docs.docker.com/compose/compose-file/)
- [GitHub Actions workflow syntax](https://docs.github.com/en/actions/using-workflows/workflow-syntax-for-github-actions)
- [Azure Web App deployment](https://github.com/marketplace/actions/azure-webapp-deploy)
```