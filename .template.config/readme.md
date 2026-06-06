```markdown
# Hướng dẫn sử dụng Template

## Cài đặt template

### 1. Clone hoặc tải template về máy

```bash
git clone https://github.com/quaqduyIT/dotnet-api-base.git
cd dotnet-api-base
```

### 2. Cài đặt template vào máy

```bash
dotnet new install .
```

## Tạo project mới từ template

### Cách 1: Tạo với tên project tùy chỉnh

```bash
dotnet new dotnet-api-base -n TenDuAnCuaBan
```

### Cách 2: Tạo vào thư mục cụ thể

```bash
dotnet new dotnet-api-base -n TenDuAn -o D:\Projects\TenDuAn
```

## Kiểm tra template đã cài

```bash
dotnet new list | findstr dotnet-api-base
```

## Gỡ template

```bash
dotnet new uninstall .
```

## Cấu trúc project sau khi tạo

```
TenDuAnCuaBan/
├── src/
│   ├── TenDuAnCuaBan.Domain/           # Domain Layer
│   ├── TenDuAnCuaBan.Application/      # Application Layer
│   ├── TenDuAnCuaBan.Infrastructure/   # Infrastructure Layer
│   ├── TenDuAnCuaBan.Shared/           # Shared Utilities
│   └── TenDuAnCuaBan.WebApi/           # WebApi Layer
├── tests/
│   ├── TenDuAnCuaBan.UnitTests/        # Unit Tests
│   └── TenDuAnCuaBan.IntegrationTests/ # Integration Tests
├── docs/
│   └── TenDuAnCuaBan.Documentation/    # Tài liệu dự án
├── .env.example                        # Mẫu cấu hình môi trường
├── Directory.Build.props               # Build settings
├── Directory.Packages.props            # Package versions
└── TenDuAnCuaBan.sln                   # Solution file
```

## Chạy project sau khi tạo

```bash
cd TenDuAnCuaBan
dotnet restore
dotnet build
cd src/TenDuAnCuaBan.WebApi
dotnet run
```

## Cập nhật template (khi có thay đổi)

```bash
# Cập nhật code template
git pull

# Gỡ template cũ
dotnet new uninstall .

# Cài lại template mới
dotnet new install .
```

## Xử lý lỗi thường gặp

### Lỗi: "No templates found matching"

```bash
# Kiểm tra template đã cài chưa
dotnet new list

# Nếu chưa, cài lại
dotnet new install .
```

### Lỗi: "The template package already contains a template with name"

```bash
# Gỡ template cũ trước khi cài mới
dotnet new uninstall .
dotnet new install .
```

## Tạo script tự động (tùy chọn)

Tạo file `create-project.ps1`:

```powershell
param([string]$ProjectName)

dotnet new dotnet-api-base -n $ProjectName
cd $ProjectName
dotnet restore
dotnet build
Write-Host "Project $ProjectName created successfully!" -ForegroundColor Green
```

Sử dụng:

```bash
.\create-project.ps1 -ProjectName "MyNewApp"
```

---

**Chúc bạn code vui vẻ! 🚀**