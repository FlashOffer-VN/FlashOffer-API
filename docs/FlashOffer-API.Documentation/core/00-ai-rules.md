# AI Rules - Quy tắc làm việc với dự án FlashOffer-API

## Mục đích

File này định nghĩa cách AI phải làm việc khi thêm feature mới vào dự án, đảm bảo AI có thể tự động theo dõi và báo cáo thay đổi.

---

## RULE 1: Template thêm feature mới

Khi được yêu cầu thêm một feature mới (ví dụ: Category, Order, User), AI phải tuân theo template sau:

### File cần TẠO MỚI (7 file)

| STT | File                                                                 | Mô tả          |
|-----|----------------------------------------------------------------------|----------------|
| 1   | `src/FlashOffer-API.Domain/Entities/{EntityName}.cs`                  | Entity class   |
| 2   | `src/FlashOffer-API.Infrastructure/Data/Configurations/{EntityName}Configuration.cs` | Entity Configuration |
| 3   | `src/FlashOffer-API.Application/DTOs/{EntityName}Dto.cs`              | Response DTO   |
| 4   | `src/FlashOffer-API.Application/DTOs/Create{EntityName}Dto.cs`        | Create DTO     |
| 5   | `src/FlashOffer-API.Application/DTOs/Update{EntityName}Dto.cs`        | Update DTO     |
| 6   | `src/FlashOffer-API.Application/Validators/{EntityName}Validators.cs` | Validator      |
| 7   | `src/FlashOffer-API.WebApi/Controllers/v1/{EntityName}Controller.cs`  | API Controller (kế thừa `CrudControllerBase`) |

### File cần SỬA ĐỔI (2 file)

| STT | File                                                            | Thay đổi            |
|-----|-----------------------------------------------------------------|---------------------|
| 1   | `src/FlashOffer-API.Application/Mappings/MappingProfile.cs`      | Thêm 3 dòng mapping |
| 2   | `src/FlashOffer-API.Infrastructure/Data/ApplicationDbContext.cs` | Thêm 1 dòng DbSet   |

### Lệnh cần CHẠY (2 lệnh)

| STT | Lệnh                                            | Mô tả             |
|-----|-------------------------------------------------|-------------------|
| 1   | `dotnet ef migrations add Add{EntityName}Table` | Tạo migration     |
| 2   | `dotnet ef database update`                     | Cập nhật database |

---

## RULE 1.1: Phân trang và Mapping
- Mọi API lấy danh sách phải hỗ trợ phân trang sử dụng `PagedList<T>`.
- Sử dụng extension `MapPagedList` để chuyển đổi PagedList giữa Entity và DTO.
- Các DTOs phải kế thừa `IMapFrom<T>` để tự động mapping.

## RULE 1.2: Repository Methods
- Các phương thức `Update` và `Delete` trong Repository là đồng bộ (không dùng Async).
- Luôn gọi `SaveChangesAsync()` để thực thi thay đổi xuống Database.

## RULE 2: Báo cáo thay đổi sau khi thêm feature

Sau khi thêm feature mới, AI phải trả về báo cáo theo format sau:

## 📋 Báo cáo thay đổi - Feature: [Tên Entity]

### 📁 File được tạo mới (Create)

| STT | File                                               | Mô tả               |
|-----|----------------------------------------------------|---------------------|
| 1   | `Domain/Entities/{EntityName}.cs`                  | Entity {EntityName} |
| 2   | `Infrastructure/Data/Configurations/{EntityName}Configuration.cs` | Entity Configuration |
| 3   | `Application/DTOs/{EntityName}Dto.cs`              | Response DTO        |
| 4   | `Application/DTOs/Create{EntityName}Dto.cs`        | Create DTO          |
| 5   | `Application/DTOs/Update{EntityName}Dto.cs`        | Update DTO          |
| 6   | `Application/Validators/{EntityName}Validators.cs` | Validator           |
| 7   | `WebApi/Controllers/v1/{EntityName}Controller.cs`  | API Controller      |

### 📝 File được sửa đổi (Modify)

| STT | File                                          | Thay đổi                 |
|-----|-----------------------------------------------|--------------------------|
| 1   | `Application/Mappings/MappingProfile.cs`      | Thêm 3 dòng mapping      |
| 2   | `Infrastructure/Data/ApplicationDbContext.cs` | Thêm DbSet<{EntityName}> |

### 🗄️ Migration

| Lệnh                                            | Trạng thái |
|-------------------------------------------------|------------|
| `dotnet ef migrations add Add{EntityName}Table` | ✅ / ❌   |
| `dotnet ef database update`                     | ✅ / ❌   |

### ✅ Checklist xác nhận

- [ ] Entity đã kế thừa BaseEntity
- [ ] DTOs đã đủ 3 loại
- [ ] Validator đã có rule cơ bản
- [ ] AutoMapper đã thêm mapping
- [ ] DbSet đã thêm
- [ ] Controller đã kế thừa ApiControllerBase
- [ ] Controller đã inject IRepository và IMapper
- [ ] GET endpoints có [AllowAnonymous]
- [ ] Migration thành công

---

## RULE 3: Cập nhật file feature

AI phải tạo file `../features/{STT}-{EntityName}.md` với nội dung:

# Feature: {EntityName} Management

## Ngày tạo
YYYY-MM-DD

## Mô tả
API quản lý {EntityName} với các chức năng CRUD cơ bản.

## Các file đã tạo

### Domain Layer
- `src/FlashOffer-API.Domain/Entities/{EntityName}.cs`

### Application Layer - DTOs
- `src/FlashOffer-API.Application/DTOs/{EntityName}Dto.cs`
- `src/FlashOffer-API.Application/DTOs/Create{EntityName}Dto.cs`
- `src/FlashOffer-API.Application/DTOs/Update{EntityName}Dto.cs`

### Application Layer - Validators
- `src/FlashOffer-API.Application/Validators/{EntityName}Validators.cs`

### WebApi Layer
- `src/FlashOffer-API.WebApi/Controllers/v1/{EntityName}Controller.cs`

## Các file đã sửa

- `src/FlashOffer-API.Application/Mappings/MappingProfile.cs` - Thêm 3 mapping
- `src/FlashOffer-API.Infrastructure/Data/ApplicationDbContext.cs` - Thêm DbSet

## Migration
- Migration name: `Add{EntityName}Table`
- Table name: `{EntityName}s`

## API Endpoints

| Method | Endpoint                      | Auth         |
|--------|-------------------------------|--------------|
| GET    | `/api/v1/{entity-lower}`      | None         |
| GET    | `/api/v1/{entity-lower}/{id}` | None         |
| POST   | `/api/v1/{entity-lower}`      | JWT Required |
| PUT    | `/api/v1/{entity-lower}/{id}` | JWT Required |
| DELETE | `/api/v1/{entity-lower}/{id}` | JWT Required |

## Ghi chú

- Đã có soft delete (IsDeleted filter)
- Đã có auto timestamp (CreatedAt, UpdatedAt)

---

## RULE 4: Cập nhật file features/README.md

AI phải thêm dòng mới vào bảng trong `../features/README.md`:

| {STT} | {EntityName} Management | [{STT}-{entity-lower}.md](./{STT}-{entity-lower}.md) | YYYY-MM-DD | ✅ Hoạt động |

---

## RULE 5: Kiểm tra tính toàn vẹn

AI phải chạy `dotnet build` và báo cáo kết quả.

---

## RULE 6: Commit thay đổi

AI phải chạy:

```bash
git add .
git commit -m "feat: add {EntityName} management API

- Added {EntityName} entity, DTOs, validator, controller
- Updated MappingProfile and DbContext
- Added migration Add{EntityName}Table
- Added feature documentation"
git push
```

---

## RULE 7: Docs sync và xác nhận

Khi có thay đổi ảnh hưởng đến cấu trúc, template hoặc quy trình khởi tạo của repo, AI phải:

1. Đảm bảo cập nhật tài liệu phù hợp trong `docs/FlashOffer-API.Documentation/` hoặc `README.md`.
2. Nếu thay đổi scripts bootstrap, prompt template, cấu hình môi trường hoặc layer core, thêm ghi chú rõ ràng vào docs.
3. Chạy `scripts/validate-docs-sync.ps1` để kiểm tra rằng nếu có thay đổi cấu trúc thì docs cũng được cập nhật.
4. Báo cáo rõ phần docs đã cập nhật trong phần “Ghi chú” của báo cáo thay đổi.

Ví dụ:
- Đã cập nhật `docs/FlashOffer-API.Documentation/guides/14-project-bootstrap.md` khi thêm `scripts/init-template.ps1`.
- Đã cập nhật `docs/FlashOffer-API.Documentation/Prompts/01-Example-Prompt.md` khi mở rộng template AI.
