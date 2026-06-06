# Troubleshooting

## Mục đích

Tài liệu này giúp giải quyết lỗi phổ biến khi chạy project local, Docker và CI/CD.

## 1. Build lỗi do .NET version

- Đảm bảo cài .NET SDK 9.0.
- Kiểm tra bằng:

```bash
dotnet --version
```

- Nếu còn hiện `8.x`, cài .NET 9.0 và chuyển môi trường.

## 2. Docker Compose không tìm thấy file

- Kiểm tra `docker-compose.yml` đã tồn tại và chưa bị ignore.
- Nếu đã bị ignore, kiểm tra `.gitignore` và dòng `docker-compose*.yml`.

## 3. SQL Server container không khởi động

- Kiểm tra logs:

```bash
docker-compose logs -f sql-server
```

- Kiểm tra biến `SA_PASSWORD` và `ACCEPT_EULA`.
- Kiểm tra volume `mssql-data` không bị lỗi.

## 4. API không kết nối database

- Kiểm tra `DB_CONNECTION_STRING` trong `.env.docker`.
- Kiểm tra tên server trong chuỗi kết nối: `sql-server`.
- Kiểm tra SQL Server đã khởi động xong.

## 5. Health check fail

- Mở browser hoặc curl:

```bash
curl http://localhost:5000/health
```

- Nếu trả lỗi 500, kiểm tra logs `dotnet-api-base`.

## 6. JWT secret quá ngắn

- `JwtSettings.Secret` cần ít nhất 32 ký tự.
- Nếu thiếu hoặc không đủ dài, app sẽ throw khi khởi động.

## 7. Swagger không hiện API

- Chỉ Swagger UI khi app chạy ở môi trường dev.
- Vào `Program.cs` để kiểm tra có `UseSwaggerUI()` trong `if (app.Environment.IsDevelopment())`.

## 8. CI không chạy

- Kiểm tra file `.github/workflows/ci.yml` có trigger đúng branch.
- Kiểm tra syntax YAML và secrets cần thiết.

## 9. Khi cần hỗ trợ nhanh

- Build local:

```bash
dotnet build
```

- Test:

```bash
dotnet test
```

- Cập nhật Docker:

```bash
docker-compose down
docker-compose up -d --build
```
