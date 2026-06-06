```markdown
# 🚀 FlashOffer-API - Base Project .NET Core API với Clean Architecture

Base project .NET Web API chuyên nghiệp với kiến trúc Clean Architecture, được thiết kế để tái sử dụng cho mọi dự án.

## 📋 Mục lục

- [Công nghệ sử dụng](#công-nghệ-sử-dụng)
- [Tính năng](#tính-năng)
- [Yêu cầu cài đặt](#yêu-cầu-cài-đặt)
- [Bắt đầu nhanh](#bắt-đầu-nhanh)
- [Cấu trúc dự án](#cấu-trúc-dự-án)
- [Cấu hình](#cấu-hình)
- [API Endpoints](#api-endpoints)
- [Xác thực JWT](#xác-thực-jwt)
- [Cách tái sử dụng cho dự án mới](#cách-tái-sử-dụng-cho-dự-án-mới)
- [Xử lý lỗi thường gặp](#xử-lý-lỗi-thường-gặp)

## 🛠 Công nghệ sử dụng

| Công nghệ | Phiên bản | Mục đích |
|-----------|-----------|----------|
| .NET | 9.0 | Runtime & Framework |
| Entity Framework Core | 9.0 | ORM - Truy cập database |
| ASP.NET Core WebAPI | 9.0 | RESTful API |
| AutoMapper | 12.0.1 | Map đối tượng (Entity ↔ DTO) |
| FluentValidation | 11.x | Validate request |
| JWT Bearer | 8.x | Xác thực người dùng |
| xUnit | 2.6.2 | Unit Testing |
| Serilog | 8.0.0 | Ghi log có cấu trúc |
| Swagger/Swashbuckle | 6.5.0 | Tài liệu API |

## ✨ Tính năng

- ✅ Clean Architecture (Domain, Application, Infrastructure, WebApi)
- ✅ Dependency Injection extensions cho từng layer
- ✅ Chuẩn hóa response (ApiResponse<T>)
- ✅ Hỗ trợ version API (v1, dễ mở rộng)
- ✅ FluentValidation (tự động validate request)
- ✅ Xác thực JWT (bảo vệ API endpoints)
- ✅ AutoMapper (tự động map Entity ↔ DTO)
- ✅ Xử lý ngoại lệ toàn cục (Global Exception)
- ✅ Ghi log với Serilog (console + file)
- ✅ Swagger/OpenAPI hỗ trợ nhiều version
- ✅ Soft delete (lọc IsDeleted)
- ✅ Tự động đánh dấu thời gian (CreatedAt, UpdatedAt)

## 📦 Yêu cầu cài đặt

Trước khi chạy project, bạn cần cài đặt:

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (hoặc SQL Server LocalDB, Docker)
- [Git](https://git-scm.com/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) hoặc [VS Code](https://code.visualstudio.com/)

### Kiểm tra cài đặt:

```bash
dotnet --version     # Phải hiển thị 9.0.x
git --version        # Kiểm tra Git
```

## 🚀 Bắt đầu nhanh

### 1. Clone repository

```bash
git clone https://github.com/TEN_CUA_BAN/dotnet-api-base.git
cd dotnet-api-base
```

### 1.1 Sử dụng repo làm template mới
Nếu bạn dùng repo này làm base project mới, chạy script rename và init:

```powershell
cd dotnet-api-base
.\scripts\rename-project.ps1 -NewProjectName YourProjectName
.\scripts\init-template.ps1
```

Nếu thay đổi cấu trúc hoặc scripts, chạy thêm:

```powershell
.\scripts\validate-docs-sync.ps1
```

### 2. Khôi phục packages

```bash
dotnet restore
```

> Xem thêm tài liệu chi tiết trong `docs/FlashOffer-API.Documentation/`.
> Xem thêm `docs/FlashOffer-API.Documentation/14-project-bootstrap.md` để sử dụng project này như một template base.

### 3. Build solution

```bash
dotnet build
```

### 4. Cấu hình kết nối database

Mở file `src/FlashOffer-API.WebApi/appsettings.json` và sửa connection string nếu cần:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FlashOffer-APIDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

> Ghi chú: bạn có thể dùng file `.env` để ghi đè cấu hình `JwtSettings` và connection string khi chạy local.

### 5. Chạy migration (tạo database)

```bash
cd src/FlashOffer-API.WebApi
dotnet ef database update
cd ../..
```

### 6. Chạy API

```bash
cd src/FlashOffer-API.WebApi
dotnet run
```

### 7. Kiểm tra API

Mở trình duyệt tại: `https://localhost:5001/swagger`

## 📁 Cấu trúc dự án

```
dotnet-api-base/
│
├── src/
│   ├── FlashOffer-API.Domain/                    # Layer 1: Domain
│   │   ├── Entities/
│   │   │   ├── BaseEntity.cs
│   │   │   └── Product.cs
│   │   └── Enums/
│   │
│   ├── FlashOffer-API.Application/               # Layer 2: Application
│   │   ├── Common/
│   │   │   └── Interfaces/
│   │   │       ├── IRepository.cs
│   │   │       ├── IApplicationDbContext.cs
│   │   │       └── IJwtService.cs
│   │   ├── DTOs/
│   │   │   ├── ProductDto.cs
│   │   │   ├── PaginationDto.cs
│   │   │   └── AuthDtos.cs
│   │   ├── Mappings/
│   │   │   └── MappingProfile.cs
│   │   ├── Validators/
│   │   │   └── ProductValidators.cs
│   │   └── DependencyInjection.cs
│   │
│   ├── FlashOffer-API.Infrastructure/            # Layer 3: Infrastructure
│   │   ├── Data/
│   │   │   └── ApplicationDbContext.cs
│   │   ├── Repositories/
│   │   │   └── GenericRepository.cs
│   │   ├── Services/
│   │   │   └── JwtService.cs
│   │   ├── Configurations/
│   │   │   └── JwtSettings.cs
│   │   └── DependencyInjection.cs
│   │
│   └── FlashOffer-API.WebApi/                    # Layer 4: WebApi
│       ├── Controllers/
│       │   ├── ApiControllerBase.cs
│       │   └── v1/
│       │       ├── SampleController.cs
│       │       └── AuthController.cs
│       ├── Middlewares/
│       │   └── GlobalExceptionMiddleware.cs
│       ├── Filters/
│       │   └── ValidationFilter.cs
│       ├── Responses/
│       │   └── ApiResponse.cs
│       ├── Configurations/
│       │   ├── ApiVersioningConfig.cs
│       │   └── SwaggerConfig.cs
│       ├── Program.cs
│       ├── DependencyInjection.cs
│       └── appsettings.json
│
├── tests/
│   ├── FlashOffer-API.UnitTests/                 # Unit Tests
│   └── FlashOffer-API.IntegrationTests/          # Integration Tests
│
├── .gitignore
├── Directory.Build.props
├── Directory.Packages.props
├── FlashOffer-API.sln
└── README.md
```

## ⚙️ Cấu hình

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FlashOffer-APIDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "JwtSettings": {
    "Secret": "YourSuperSecretKeyHereAtLeast32CharactersLong!",
    "Issuer": "FlashOffer-API",
    "Audience": "FlashOffer-APIClient",
    "ExpiryMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### JWT Secret

**Quan trọng:** Thay đổi `Secret` trong `JwtSettings` thành khóa bí mật của riêng bạn (ít nhất 32 ký tự).

## 🔌 API Endpoints

### Auth Endpoints

| Method | Endpoint | Mô tả | Xác thực |
|--------|----------|-------|----------|
| POST | `/api/v1/auth/login` | Đăng nhập lấy token | Không |

**Login request:**
```json
{
  "username": "admin",
  "password": "password"
}
```

**Login response:**
```json
{
  "success": true,
  "message": "Success",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIs...",
    "expiresAt": "2024-01-01T00:00:00Z",
    "username": "admin"
  }
}
```

### Sample Endpoints

| Method | Endpoint | Mô tả | Xác thực |
|--------|----------|-------|----------|
| GET | `/api/v1/sample` | Lấy tất cả sản phẩm | Không |
| GET | `/api/v1/sample/{id}` | Lấy sản phẩm theo id | Không |
| POST | `/api/v1/sample` | Tạo sản phẩm mới | Cần JWT |
| PUT | `/api/v1/sample/{id}` | Cập nhật sản phẩm | Cần JWT |
| DELETE | `/api/v1/sample/{id}` | Xóa sản phẩm | Cần JWT |

## 🔐 Xác thực JWT

### Cách lấy token:

```bash
curl -X POST https://localhost:5001/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "password"}'
```

### Cách dùng token:

```bash
curl -X GET https://localhost:5001/api/v1/sample \
  -H "Authorization: Bearer TOKEN_CUA_BAN"
```

## 🔄 Cách tái sử dụng cho dự án mới

### Cách 1: Clone và đổi tên (nhanh nhất)

```bash
# Clone base project
git clone https://github.com/TEN_CUA_BAN/dotnet-api-base.git TenDuAnMoi
cd TenDuAnMoi

# Xóa thư mục .git để tạo repo mới
rm -rf .git

# Khởi tạo Git mới
git init
git add .
git commit -m "Initial commit from base template"

# Tạo repo mới trên GitHub và push
git remote add origin https://github.com/TEN_CUA_BAN/TenDuAnMoi.git
git push -u origin main
```

### Cách 2: Đổi namespace (PowerShell - Windows)

```powershell
# Đổi tên solution
ren FlashOffer-API.sln TenDuAnMoi.sln

# Đổi namespace trong tất cả file .cs
Get-ChildItem -Recurse -Include *.cs | ForEach-Object {
    (Get-Content $_.FullName) -replace 'FlashOffer-API', 'TenDuAnMoi' | Set-Content $_.FullName
}

# Đổi tên thư mục
ren src\FlashOffer-API.Domain src\TenDuAnMoi.Domain
ren src\FlashOffer-API.Application src\TenDuAnMoi.Application
ren src\FlashOffer-API.Infrastructure src\TenDuAnMoi.Infrastructure
ren src\FlashOffer-API.WebApi src\TenDuAnMoi.WebApi
ren tests\FlashOffer-API.UnitTests tests\TenDuAnMoi.UnitTests
ren tests\FlashOffer-API.IntegrationTests tests\TenDuAnMoi.IntegrationTests
```

### Cách 3: Cập nhật file .csproj

Sau khi đổi tên thư mục, cập nhật từng file `.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <RootNamespace>TenDuAnMoi.Domain</RootNamespace>
    <AssemblyName>TenDuAnMoi.Domain</AssemblyName>
  </PropertyGroup>
</Project>
```

### Cách 4: Cập nhật solution references

Mở solution trong Visual Studio hoặc VS Code, sau đó chuột phải vào Solution → Add → Existing Project, chọn các project đã đổi tên.

## 🗄️ Migration database cho dự án mới

Sau khi đổi tên và cấu hình connection string:

```bash
cd src/TenDuAnMoi.WebApi

# Xóa migrations cũ (nếu có)
rm -rf ../TenDuAnMoi.Infrastructure/Migrations

# Tạo migration mới
dotnet ef migrations add InitialCreate --context ApplicationDbContext

# Cập nhật database
dotnet ef database update --context ApplicationDbContext

cd ../..
```

## 🧪 Chạy kiểm thử

```bash
# Chạy tất cả test
dotnet test

# Chạy test với độ phủ code
dotnet test --collect:"XPlat Code Coverage"
```

## 🔧 Xử lý lỗi thường gặp

### Lỗi: "dotnet ef not found"

```bash
dotnet tool install --global dotnet-ef
```

### Lỗi: "Cannot connect to database"

1. Kiểm tra SQL Server đang chạy:
   ```bash
   sqlcmd -S (localdb)\mssqllocaldb -Q "SELECT 1"
   ```

2. Cập nhật connection string trong `appsettings.json`

### Lỗi: "Build failed - warnings as errors"

Trong `Directory.Build.props`, tạm thời set:
```xml
<TreatWarningsAsErrors>false</TreatWarningsAsErrors>
```

### Lỗi: JWT token không hợp lệ

Đảm bảo `Secret` trong `appsettings.json` dài ít nhất 32 ký tự.

## 📝 Giấy phép

Sử dụng nội bộ

## 👥 Người đóng góp

Tên của bạn - Công việc ban đầu

---

## 🎯 Kế hoạch phát triển (tùy chọn)

- [ ] Redis Caching
- [ ] Hỗ trợ Docker
- [ ] CI/CD pipeline (GitHub Actions)
- [ ] Serilog với Seq/Elasticsearch
- [ ] Health checks endpoint
- [ ] Giới hạn tốc độ (Rate limiting)
- [ ] Background services

---

**Chúc bạn code vui vẻ! 🚀**
```