```markdown
# ðŸš€ FlashOffer.API - Base Project .NET Core API vá»›i Clean Architecture

Base project .NET Web API chuyÃªn nghiá»‡p vá»›i kiáº¿n trÃºc Clean Architecture, Ä‘Æ°á»£c thiáº¿t káº¿ Ä‘á»ƒ tÃ¡i sá»­ dá»¥ng cho má»i dá»± Ã¡n.

## ðŸ“‹ Má»¥c lá»¥c

- [CÃ´ng nghá»‡ sá»­ dá»¥ng](#cÃ´ng-nghá»‡-sá»­-dá»¥ng)
- [TÃ­nh nÄƒng](#tÃ­nh-nÄƒng)
- [YÃªu cáº§u cÃ i Ä‘áº·t](#yÃªu-cáº§u-cÃ i-Ä‘áº·t)
- [Báº¯t Ä‘áº§u nhanh](#báº¯t-Ä‘áº§u-nhanh)
- [Cáº¥u trÃºc dá»± Ã¡n](#cáº¥u-trÃºc-dá»±-Ã¡n)
- [Cáº¥u hÃ¬nh](#cáº¥u-hÃ¬nh)
- [API Endpoints](#api-endpoints)
- [XÃ¡c thá»±c JWT](#xÃ¡c-thá»±c-jwt)
- [CÃ¡ch tÃ¡i sá»­ dá»¥ng cho dá»± Ã¡n má»›i](#cÃ¡ch-tÃ¡i-sá»­-dá»¥ng-cho-dá»±-Ã¡n-má»›i)
- [Xá»­ lÃ½ lá»—i thÆ°á»ng gáº·p](#xá»­-lÃ½-lá»—i-thÆ°á»ng-gáº·p)

## ðŸ›  CÃ´ng nghá»‡ sá»­ dá»¥ng

| CÃ´ng nghá»‡                     | PhiÃªn báº£n | Má»¥c Ä‘Ã­ch                               |
|-------------------------------|-----------|----------------------------------------|
| .NET                          | 9.0       | Runtime & Framework                    |
| Entity Framework Core         | 8.0       | ORM - Truy cáº­p database                |
| ASP.NET Core WebAPI           | 9.0       | RESTful API                            |
| AutoMapper                    | 12.0.1    | Map Ä‘á»‘i tÆ°á»£ng (Entity â†” DTO)           |
| FluentValidation              | 11.x      | Validate request                       |
| JWT Bearer                    | 8.x       | XÃ¡c thá»±c ngÆ°á»i dÃ¹ng                    |
| xUnit                         | 2.6.2     | Unit Testing                           |
| Serilog                       | 8.0.0     | Ghi log cÃ³ cáº¥u trÃºc                    |
| Swagger/Swashbuckle           | 6.5.0     | TÃ i liá»‡u API                           |

## âœ¨ TÃ­nh nÄƒng

- âœ… Clean Architecture (Domain, Application, Infrastructure, WebApi)
- âœ… Dependency Injection extensions cho tá»«ng layer
- âœ… Chuáº©n hÃ³a response (ApiResponse<T>)
- âœ… Há»— trá»£ version API (v1, dá»… má»Ÿ rá»™ng)
- âœ… FluentValidation (tá»± Ä‘á»™ng validate request)
- âœ… XÃ¡c thá»±c JWT (báº£o vá»‡ API endpoints)
- âœ… AutoMapper (tá»± Ä‘á»™ng map Entity â†” DTO)
- âœ… Xá»­ lÃ½ ngoáº¡i lá»‡ toÃ n cá»¥c (Global Exception)
- âœ… Ghi log vá»›i Serilog (console + file)
- âœ… Swagger/OpenAPI há»— trá»£ nhiá»u version
- âœ… Soft delete (lá»c IsDeleted)
- âœ… Tá»± Ä‘á»™ng Ä‘Ã¡nh dáº¥u thá»i gian (CreatedAt, UpdatedAt)

## ðŸ“¦ YÃªu cáº§u cÃ i Ä‘áº·t

TrÆ°á»›c khi cháº¡y project, báº¡n cáº§n cÃ i Ä‘áº·t:

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (hoáº·c SQL Server LocalDB, Docker)
- [Git](https://git-scm.com/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) hoáº·c [VS Code](https://code.visualstudio.com/)

### Kiá»ƒm tra cÃ i Ä‘áº·t:

```bash
dotnet --version     # Pháº£i hiá»ƒn thá»‹ 9.0.x
git --version        # Kiá»ƒm tra Git
```

## ðŸš€ Báº¯t Ä‘áº§u nhanh

### 1. Clone repository

```bash
git clone https://github.com/TEN_CUA_BAN/dotnet-api-base.git
cd dotnet-api-base
```

### 2. KhÃ´i phá»¥c packages

```bash
dotnet restore
```

### 3. Build solution

```bash
dotnet build
```

### 4. Cáº¥u hÃ¬nh káº¿t ná»‘i database

Má»Ÿ file `src/FlashOffer.API.WebApi/appsettings.json` vÃ  sá»­a connection string náº¿u cáº§n:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FlashOffer.APIDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### 5. Cháº¡y migration (táº¡o database)

```bash
cd src/FlashOffer.API.WebApi
dotnet ef database update
cd ../..
```

### 6. Cháº¡y API

```bash
cd src/FlashOffer.API.WebApi
dotnet run
```

### 7. Kiá»ƒm tra API

Má»Ÿ trÃ¬nh duyá»‡t táº¡i: `https://localhost:5001/swagger`

## ðŸ“ Cáº¥u trÃºc dá»± Ã¡n

```
dotnet-api-base/
â”‚
â”œâ”€â”€ docs/
â”‚   â””â”€â”€ FlashOffer.API.Documentation/       # TÃ i liá»‡u dá»± Ã¡n
â”‚
â”œâ”€â”€ src/
â”‚   â”œâ”€â”€ FlashOffer.API.Domain/              # Layer 1: Domain
â”‚   â”œâ”€â”€ FlashOffer.API.Application/         # Layer 2: Application
â”‚   â”œâ”€â”€ FlashOffer.API.Infrastructure/      # Layer 3: Infrastructure
â”‚   â””â”€â”€ FlashOffer.API.WebApi/              # Layer 4: WebApi
â”‚
â”œâ”€â”€ tests/
â”‚   â”œâ”€â”€ FlashOffer.API.UnitTests/           # Unit Tests
â”‚   â””â”€â”€ FlashOffer.API.IntegrationTests/    # Integration Tests
â”‚
â”œâ”€â”€ .env                                   # Cáº¥u hÃ¬nh mÃ´i trÆ°á»ng (khÃ´ng commit)
â”œâ”€â”€ .env.example                           # Máº«u cáº¥u hÃ¬nh mÃ´i trÆ°á»ng
â”œâ”€â”€ .gitignore
â”œâ”€â”€ Directory.Build.props
â”œâ”€â”€ Directory.Packages.props
â”œâ”€â”€ FlashOffer.API.sln
â””â”€â”€ README.md
```

## âš™ï¸ Cáº¥u hÃ¬nh

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FlashOffer.APIDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "JwtSettings": {
    "Secret": "YourSuperSecretKeyHereAtLeast32CharactersLong!",
    "Issuer": "FlashOffer.API",
    "Audience": "FlashOffer.APIClient",
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

### Cáº¥u hÃ¬nh .env (Æ°u tiÃªn cao hÆ¡n appsettings.json)

Táº¡o file `.env` tá»« `.env.example` vÃ  Ä‘iá»n giÃ¡ trá»‹ tháº­t:

```
DB_CONNECTION_STRING=Server=(localdb)\\mssqllocaldb;Database=YourDatabase;Trusted_Connection=True
JWT_SECRET=YourSuperSecretKeyHereAtLeast32CharactersLong!
JWT_ISSUER=YourAppName
JWT_AUDIENCE=YourAppClient
JWT_EXPIRY_MINUTES=60
LOG_LEVEL=Information
```

### JWT Secret

**Quan trá»ng:** Thay Ä‘á»•i `Secret` trong `JwtSettings` hoáº·c `.env` thÃ nh khÃ³a bÃ­ máº­t cá»§a riÃªng báº¡n (Ã­t nháº¥t 32 kÃ½ tá»±).

## ðŸ”Œ API Endpoints

### Auth Endpoints

| Method | Endpoint                   | MÃ´ táº£                  | XÃ¡c thá»±c |
|--------|----------------------------|------------------------|----------|
| POST   | `/api/v1/auth/login`       | ÄÄƒng nháº­p láº¥y token    | KhÃ´ng    |

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

| Method | Endpoint                           | MÃ´ táº£                     | XÃ¡c thá»±c      |
|--------|------------------------------------|---------------------------|---------------|
| GET    | `/api/v1/sample`                   | Láº¥y táº¥t cáº£ sáº£n pháº©m       | KhÃ´ng         |
| GET    | `/api/v1/sample/{id}`              | Láº¥y sáº£n pháº©m theo id      | KhÃ´ng         |
| POST   | `/api/v1/sample`                   | Táº¡o sáº£n pháº©m má»›i          | Cáº§n JWT       |
| PUT    | `/api/v1/sample/{id}`              | Cáº­p nháº­t sáº£n pháº©m         | Cáº§n JWT       |
| DELETE | `/api/v1/sample/{id}`              | XÃ³a sáº£n pháº©m              | Cáº§n JWT       |

## ðŸ” XÃ¡c thá»±c JWT

### CÃ¡ch láº¥y token:

```bash
curl -X POST https://localhost:5001/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "password"}'
```

### CÃ¡ch dÃ¹ng token:

```bash
curl -X GET https://localhost:5001/api/v1/sample \
  -H "Authorization: Bearer TOKEN_CUA_BAN"
```

## ðŸ”„ CÃ¡ch tÃ¡i sá»­ dá»¥ng cho dá»± Ã¡n má»›i

### CÃ¡ch 1: Clone vÃ  Ä‘á»•i tÃªn (nhanh nháº¥t)

```bash
# Clone base project
git clone https://github.com/TEN_CUA_BAN/dotnet-api-base.git TenDuAnMoi
cd TenDuAnMoi

# XÃ³a thÆ° má»¥c .git Ä‘á»ƒ táº¡o repo má»›i
rm -rf .git

# Khá»Ÿi táº¡o Git má»›i
git init
git add .
git commit -m "Initial commit from base template"

# Táº¡o repo má»›i trÃªn GitHub vÃ  push
git remote add origin https://github.com/TEN_CUA_BAN/TenDuAnMoi.git
git push -u origin main
```

### CÃ¡ch 2: Äá»•i namespace (PowerShell - Windows)

```powershell
# Äá»•i tÃªn solution
ren FlashOffer.API.sln TenDuAnMoi.sln

# Äá»•i namespace trong táº¥t cáº£ file .cs
Get-ChildItem -Recurse -Include *.cs | ForEach-Object {
    (Get-Content $_.FullName) -replace 'FlashOffer.API', 'TenDuAnMoi' | Set-Content $_.FullName
}

# Äá»•i tÃªn thÆ° má»¥c
ren src\FlashOffer.API.Domain src\TenDuAnMoi.Domain
ren src\FlashOffer.API.Application src\TenDuAnMoi.Application
ren src\FlashOffer.API.Infrastructure src\TenDuAnMoi.Infrastructure
ren src\FlashOffer.API.WebApi src\TenDuAnMoi.WebApi
ren tests\FlashOffer.API.UnitTests tests\TenDuAnMoi.UnitTests
ren tests\FlashOffer.API.IntegrationTests tests\TenDuAnMoi.IntegrationTests
ren docs\FlashOffer.API.Documentation docs\TenDuAnMoi.Documentation
```

### CÃ¡ch 3: Cáº­p nháº­t file .csproj

Sau khi Ä‘á»•i tÃªn thÆ° má»¥c, cáº­p nháº­t tá»«ng file `.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <RootNamespace>TenDuAnMoi.Domain</RootNamespace>
    <AssemblyName>TenDuAnMoi.Domain</AssemblyName>
  </PropertyGroup>
</Project>
```

### CÃ¡ch 4: Cáº­p nháº­t solution references

Má»Ÿ solution trong Visual Studio hoáº·c VS Code, sau Ä‘Ã³ chuá»™t pháº£i vÃ o Solution â†’ Add â†’ Existing Project, chá»n cÃ¡c project Ä‘Ã£ Ä‘á»•i tÃªn.

## ðŸ—„ï¸ Migration database cho dá»± Ã¡n má»›i

Sau khi Ä‘á»•i tÃªn vÃ  cáº¥u hÃ¬nh connection string:

```bash
cd src/TenDuAnMoi.WebApi

# XÃ³a migrations cÅ© (náº¿u cÃ³)
rm -rf ../TenDuAnMoi.Infrastructure/Migrations

# Táº¡o migration má»›i
dotnet ef migrations add InitialCreate --context ApplicationDbContext

# Cáº­p nháº­t database
dotnet ef database update --context ApplicationDbContext

cd ../..
```

## ðŸ§ª Cháº¡y kiá»ƒm thá»­

```bash
# Cháº¡y táº¥t cáº£ test
dotnet test

# Cháº¡y test vá»›i Ä‘á»™ phá»§ code
dotnet test --collect:"XPlat Code Coverage"
```

## ðŸ”§ Xá»­ lÃ½ lá»—i thÆ°á»ng gáº·p

### Lá»—i: "dotnet ef not found"

```bash
dotnet tool install --global dotnet-ef
```

### Lá»—i: "Cannot connect to database"

1. Kiá»ƒm tra SQL Server Ä‘ang cháº¡y:

```bash
sqlcmd -S (localdb)\mssqllocaldb -Q "SELECT 1"
```

2. Cáº­p nháº­t connection string trong `appsettings.json` hoáº·c `.env`

### Lá»—i: "Build failed - warnings as errors"

Trong `Directory.Build.props`, táº¡m thá»i set:

```xml
<TreatWarningsAsErrors>false</TreatWarningsAsErrors>
```

### Lá»—i: JWT token khÃ´ng há»£p lá»‡

Äáº£m báº£o `Secret` trong `appsettings.json` hoáº·c `.env` dÃ i Ã­t nháº¥t 32 kÃ½ tá»±.

## ðŸ“ Giáº¥y phÃ©p

Sá»­ dá»¥ng ná»™i bá»™

## ðŸ‘¥ NgÆ°á»i Ä‘Ã³ng gÃ³p

TÃªn cá»§a báº¡n - CÃ´ng viá»‡c ban Ä‘áº§u

---

## ðŸŽ¯ Káº¿ hoáº¡ch phÃ¡t triá»ƒn (tÃ¹y chá»n)

- [ ] Redis Caching
- [ ] Há»— trá»£ Docker
- [ ] CI/CD pipeline (GitHub Actions)
- [ ] Serilog vá»›i Seq/Elasticsearch
- [ ] Health checks endpoint
- [ ] Giá»›i háº¡n tá»‘c Ä‘á»™ (Rate limiting)
- [ ] Background services

---

**ChÃºc báº¡n code vui váº»! ðŸš€**
```