# Project bootstrap

## Mục đích

File này hướng dẫn cách dùng `dotnet-api-base` như một starter template để khởi tạo API mới nhanh chóng.

## Khi nào dùng

- Khi bạn muốn tạo dự án API mới mà không phải cấu hình lại các phần cơ bản.
- Khi cần giữ lại kiến trúc Clean Architecture, DI, validation, JWT, logging.
- Khi muốn sử dụng lại template với ít bước setup nhất.

## Bắt đầu nhanh bằng script

### 1. Clone repo

```bash
git clone https://github.com/TEN_CUA_BAN/dotnet-api-base.git TenDuAnMoi
cd TenDuAnMoi
```

### 2. Đổi tên project tự động

Chạy script đổi tên để cập nhật folder, file và nội dung:

```powershell
.\scripts\rename-project.ps1 -NewProjectName TenDuAnMoi
```

### 3. Khởi tạo template

Tiếp theo, tạo file cấu hình `.env` nếu cần và restore + build:

```powershell
.\scripts\init-template.ps1
```

Nếu thay đổi cấu trúc repo hoặc scripts bootstrap, nhớ chạy thêm:

```powershell
.\scripts\validate-docs-sync.ps1
```

Bạn có thể thêm tùy chọn:

```powershell
.\scripts\init-template.ps1 -UseDockerEnv -RunTests
```

## Nếu muốn giữ Git history cũ

Nếu bạn không cần giữ lịch sử Git cũ, xóa thư mục `.git` và tạo repository mới:

```bash
rm -rf .git
git init
git add .
git commit -m "Initial commit from FlashOffer-API"
```

## Cấu hình môi trường

Nếu bạn dùng file `.env.example`, `init-template.ps1` sẽ sao chép nó thành `.env` tự động.

Nếu muốn dùng Docker env:

```powershell
.\scripts\init-template.ps1 -UseDockerEnv
```

## Kiểm tra project

- Chạy restore và build

```bash
dotnet restore TenDuAnMoi.slnx
dotnet build TenDuAnMoi.slnx
```

- Chạy API

```bash
cd src\TenDuAnMoi.WebApi
dotnet run
```

## Checklist template

- [ ] Đổi tên solution và project folder
- [ ] Đổi tên csproj và namespace trong nội dung file
- [ ] Tạo file `.env` từ `.env.example`
- [ ] Build solution
- [ ] Chạy test nếu cần
- [ ] Cập nhật `README.md` và docs sau khi rename

## Gợi ý

- Giữ lại cấu trúc `src/Domain`, `src/Application`, `src/Infrastructure`, `src/WebApi`.
- Nếu dự án mới không cần `tests/`, bạn có thể xóa các thư mục test.
- Nếu cần Docker, copy `docker-compose.yml` và `.env.docker.example`.

## Tham khảo

- `README.md` phần `Cách tái sử dụng cho dự án mới`
- `scripts/rename-project.ps1`
- `scripts/init-template.ps1`
