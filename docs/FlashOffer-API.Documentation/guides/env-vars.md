# Biến môi trường

## Mục đích

Tài liệu này mô tả các biến môi trường được sử dụng bởi project và cách config chúng.

## Các file mẫu

- `.env.example`
- `.env.docker.example`

## Quy tắc chung

- `.env` và `.env.*` bị ignore bởi Git để không commit secrets.
- `.env.example` và `.env.docker.example` được giữ trong repo làm mẫu.
- `DotNetEnv` được dùng trong `Program.cs` để tải giá trị từ `.env`.
- Environment variables cũng có thể được supply trực tiếp vào hệ thống.

## Biến bắt buộc

| Biến | Ý nghĩa | Ví dụ |
|------|---------|------|
| `DB_CONNECTION_STRING` | Chuỗi kết nối SQL Server | `Server=sql-server;Database=FlashOffer-APIDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;` |
| `JWT_SECRET` | Khóa bí mật JWT (ít nhất 32 ký tự) | `YourSuperSecretKeyHereAtLeast32CharactersLong!` |
| `JWT_ISSUER` | Issuer cho token JWT | `FlashOffer-API` |
| `JWT_AUDIENCE` | Audience cho token JWT | `FlashOffer-APIClient` |
| `JWT_EXPIRY_MINUTES` | Thời hạn token (phút) | `60` |

## Biến tùy chọn

| Biến | Ý nghĩa | Mặc định |
|------|---------|--------|
| `LOG_LEVEL` | Mức log Serilog | `Information` |
| `SA_PASSWORD` | Mật khẩu SA cho SQL Server | `YourStrong!Passw0rd` |
| `ACCEPT_EULA` | Chấp nhận điều khoản SQL Server | `Y` |

## Tương tác với Docker Compose

- `docker-compose.yml` dùng file `.env.docker` để load giá trị.
- Bạn cần tạo file từ mẫu:

```bash
cp .env.docker.example .env.docker
```

- Sau đó sửa giá trị phù hợp.

## Lưu ý bảo mật

- Không commit `.env.docker` hoặc `.env` thực tế.
- Dùng secret manager khi deploy production.
