# QUY TẮC TRẢ LỜI CHO DỰ ÁN FLASHOFFER

## 1. Quy tắc chung

- Luôn trả lời bằng tiếng Việt, trừ code và thuật ngữ chuyên môn.
- Mỗi câu trả lời tối đa 30 dòng (không tính code block).
- Không lặp lại nội dung đã nói ở câu trước.
- Thứ tự ưu tiên: Kết quả/Phân tích > Hành động tiếp theo > Giải thích chi tiết.

## 2. Khi hướng dẫn code / làm dự án / xây dựng tính năng

| Bước | Hành động |
|------|-----------|
| 1 | Nêu tổng quan 2-3 câu |
| 2 | Liệt kê cách tiếp cận (bảng hoặc bullet), kèm ưu/nhược điểm |
| 3 | Hỏi người dùng chọn hướng |
| 4 | **SAU KHI CONFIRM** mới hướng dẫn chi tiết (kèm code mẫu) |
| 5 | Chờ xác nhận xong bước hiện tại rồi mới chuyển tiếp |

**KHÔNG:** gộp code các bước, tự động chuyển bước, thêm bước thừa.

## 3. Khi gặp lỗi cần debug nhiều bước

| Bước | Hành động |
|------|-----------|
| 1 | Đưa giả thuyết + 1 câu lệnh kiểm tra đầu tiên |
| 2 | Chờ người dùng báo kết quả |
| 3 | Phân tích kết quả trong khung `**Phân tích:**` |
| 4 | Hỏi "Bạn muốn tiếp tục hay dừng lại?" |
| 5 | Lặp lại đến khi xác định nguyên nhân gốc rễ |
| 6 | **SAU KHI xác định nguyên nhân** mới đưa giải pháp |

**KHÔNG:** đoán mò, gộp kiểm tra, đưa giải pháp khi chưa rõ nguyên nhân.

### Debug 500 Error trong Integration Test

| Status Code | Nguyên nhân | Giải pháp |
|-------------|-------------|-----------|
| 500 khi valid request | Thiếu AutoMapper mapping | Thêm mapping trong MappingProfile hoặc IMapFrom |
| 500 khi invalid request | Xung đột database provider | Xóa hết DbContext registrations trước khi add InMemory |

## 4. Code mẫu

- Backend: C# với syntax highlighting ` ```csharp `
- Frontend: TypeScript (Angular)
- Database: SQL có bảng Markdown kết quả

```csharp
// Code phải chạy được, có comment giải thích
```

## 5. Khi viết Issue cho API

**Format trả lời:** CHỈ nội dung issue, KHÔNG lời dẫn hay giải thích.

```markdown
## ✨ Implement API [METHOD] /[đường dẫn] - [mô tả ngắn]

### 📌 Mục tiêu
[mô tả ngắn]

### 🔗 Endpoint

| Property | Giá trị |
|----------|---------|
| Method | [METHOD] |
| URL | [đường dẫn] |
| Auth | [Có/Không yêu cầu] |

### 📦 Request Body

```json
{ ... }
```

### 📋 Validation Rules

| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| ... | ... | ... |

### ✅ Response (200 OK)

```json
{ ... }
```

### ❌ Error Response (400)

```json
{ ... }
```

### 📝 Acceptance Criteria

- [ ] ...
```

## 6. Thông tin dự án FlashOffer

### Tổng quan

| Mục | Nội dung |
|-----|----------|
| Tên dự án | FlashOffer |
| Mô tả | Nền tảng kết nối cung cầu, mua chung, nhận offer giảm giá, CTV bán hàng |
| Tech stack | .NET 9, ASP.NET Core WebAPI, SQLite/SQL Server, JWT, Serilog |
| Kiến trúc | Clean Architecture (Domain, Application, Infrastructure, WebApi) |

### Cấu trúc thư mục

| Layer | Thư mục | Vai trò |
|-------|---------|---------|
| Domain | `src/FlashOffer.API.Domain` | Entities, Enums |
| Application | `src/FlashOffer.API.Application` | Common/Interfaces, Validators, Mappings, Features, Resources |
| Application | `src/FlashOffer.API.Application/DTOs/requests/` | Create/Update DTOs |
| Application | `src/FlashOffer.API.Application/DTOs/responses/` | Response DTOs |
| Application | `src/FlashOffer.API.Application/DTOs/common/` | Shared DTOs (PagedResult) |
| Infrastructure | `src/FlashOffer.API.Infrastructure` | DbContext, Repository, Services |
| Infrastructure | `src/FlashOffer.API.Infrastructure/Configurations/` | App settings (JWT, Email, Stripe) |
| Infrastructure | `src/FlashOffer.API.Infrastructure/Data/Configurations/` | Entity Framework mappings |
| WebApi | `src/FlashOffer.API.WebApi` | Controllers, Middlewares, Filters |
| Shared | `src/FlashOffer.API.Shared` | Common/Interfaces, Extensions, Helpers |

### Namespace mapping (QUAN TRỌNG)

| Class/Interface | Namespace |
|----------------|-----------|
| `BaseEntity` | `FlashOffer.API.Domain.Entities` |
| `IRepository<T>` | `FlashOffer.API.Domain.Interfaces` |
| `IApplicationDbContext` | `FlashOffer.API.Domain.Interfaces` |
| `PagedList<T>` | `FlashOffer.API.Domain.Models` |
| `ICurrentUserService` | `FlashOffer.API.Shared.Common.Interfaces` |
| `IJwtService` | `FlashOffer.API.Shared.Common.Interfaces` |
| `IMapFrom<T>` | `FlashOffer.API.Application.Common.Mappings` |
| `IAuthService` | `FlashOffer.API.Application.Common.Interfaces` |
| `ApiControllerBase` | `FlashOffer.API.WebApi` |
| `SharedResource` | `FlashOffer.API.Application.Resources` |

### Quy tắc phát triển

#### 1. DTOs & Mapping (AutoMapper) - BẮT BUỘC
- Request/Response DTOs implement `IMapFrom<TEntity>`
- Commands trong MediatR cũng phải implement `IMapFrom<T>`

```csharp
// Request DTO
public class CreateXxxDto : IMapFrom<XxxEntity>
{
    public void Mapping(Profile profile) 
        => profile.CreateMap<CreateXxxDto, XxxEntity>();
}

// Response DTO
public class XxxResponseDto : IMapFrom<XxxEntity>
{
    public void Mapping(Profile profile)
        => profile.CreateMap<XxxEntity, XxxResponseDto>();
}

// Command (MediatR)
public class CreateXxxCommand : IRequest<XxxResponseDto>, IMapFrom<CreateXxxDto>
{
    public void Mapping(Profile profile)
        => profile.CreateMap<CreateXxxDto, CreateXxxCommand>();
}
```

#### 2. Validation (FluentValidation)
- Inject `IStringLocalizer<SharedResource>` cho message đa ngôn ngữ
- Dùng resource key, không hardcode message

**Quy tắc cho Phone validation (tránh lỗi trùng lặp):**
```csharp
// ✅ ĐÚNG
RuleFor(x => x.Phone)
    .NotEmpty().WithMessage(localizer["PhoneRequired"]);

RuleFor(x => x.Phone)
    .Must(phone => string.IsNullOrEmpty(phone) || System.Text.RegularExpressions.Regex.IsMatch(phone, @"^0[0-9]{9,10}$"))
    .WithMessage(localizer["PhoneInvalid"]);

// ❌ SAI - Khi Phone rỗng, rule không chạy
RuleFor(x => x.Phone)
    .NotEmpty().WithMessage(localizer["PhoneRequired"])
    .Must(phone => Regex.IsMatch(phone, @"^0[0-9]{9,10}$"))
    .WithMessage(localizer["PhoneInvalid"])
    .When(x => !string.IsNullOrEmpty(x.Phone));
```

#### 3. Resource Keys
- **Chỉ thêm key mới** khi chưa tồn tại trong hệ thống
- Key đã có (ProductNameRequired, PhoneInvalid...) tái sử dụng

| Loại | Format | Ví dụ |
|------|--------|-------|
| Success | `{Action}{Feature}Success` | `CreateOrderSuccess` |
| Not Found | `{Feature}NotFound` | `OrderNotFound` |
| Validation | `{FieldName}Rule` | `PricePositive` |

#### 4. Enum (cho trạng thái, loại, danh mục)
- Đặt trong thư mục `Domain/Enums/`
- Entity dùng enum thay vì string
- EF Configuration dùng `HasConversion<int>()`

```csharp
// Enum
public enum OrderStatus { Pending = 1, Confirmed = 2 }

// Entity
public OrderStatus Status { get; set; }

// Config
builder.Property(x => x.Status)
    .HasConversion<int>()
    .HasDefaultValue(OrderStatus.Pending);
```

#### 5. Service Layer - 2 cách tiếp cận

| Cách | Đường dẫn | Phù hợp |
|------|-----------|---------|
| Service trực tiếp | `I{Feature}Service` / `{Feature}Service` | CRUD đơn giản |
| MediatR CQRS | `Features/{Feature}/Commands|Queries|Handlers` | Logic phức tạp |

**Quy tắc chọn Service vs MediatR:**

| Tiêu chí | Service | MediatR |
|----------|---------|---------|
| CRUD đơn giản | ✅ | ❌ |
| Cần cross-cutting concerns | ❌ | ✅ |
| Số lượng method | 1-3 | >5 |
| Độ phức tạp | Thấp | Cao |

#### 6. Repository methods có sẵn (ĐÃ CẬP NHẬT)

| Method | Mô tả |
|--------|-------|
| `GetByIdAsync(Guid id)` | Lấy theo Id (auto filter IsDeleted) |
| `GetAllAsync()` | Lấy tất cả |
| `FindAsync(predicate)` | Lấy thỏa điều kiện |
| `GetPagedAsync(page, size, predicate)` | Phân trang cơ bản |
| `GetPagedWithOrderAsync(page, size, predicate, orderBy, isDescending)` | Phân trang + sắp xếp |
| `GetPagedWithIncludesAsync(page, size, includes, predicate, orderBy, isDescending)` | Phân trang + Include navigation properties |
| `AddAsync(entity)` | Thêm mới |
| `Update(entity)` | Cập nhật |
| `Delete(entity)` | Xóa mềm |
| `SaveChangesAsync()` | Lưu thay đổi |

**Interface IRepository đầy đủ:**

```csharp
using System.Linq.Expressions;
using FlashOffer.API.Domain.Models;

namespace FlashOffer.API.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    // ✅ Phân trang cơ bản
    Task<PagedList<T>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    // ✅ Phân trang + sắp xếp
    Task<PagedList<T>> GetPagedWithOrderAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? predicate,
        Expression<Func<T, object>>? orderBy,
        bool isDescending = true,
        CancellationToken cancellationToken = default);

    // ✅ Phân trang + Include + sắp xếp
    Task<PagedList<T>> GetPagedWithIncludesAsync(
        int pageNumber,
        int pageSize,
        Func<IQueryable<T>, IQueryable<T>>? includes = null,
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>? orderBy = null,
        bool isDescending = true,
        CancellationToken cancellationToken = default);

    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Delete(T entity);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
```

**GenericRepository Implementation:**

```csharp
using FlashOffer.API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using FlashOffer.API.Domain.Interfaces;

namespace FlashOffer.API.Infrastructure.Repositories;

public class GenericRepository<T> : IRepository<T> where T : class
{
    protected readonly IApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(IApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<PagedList<T>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbSet;
        if (predicate != null)
            query = query.Where(predicate);
        return await PagedList<T>.CreateAsync(query, pageNumber, pageSize);
    }

    public async Task<PagedList<T>> GetPagedWithOrderAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? predicate,
        Expression<Func<T, object>>? orderBy,
        bool isDescending = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbSet;
        if (predicate != null)
            query = query.Where(predicate);
        if (orderBy != null)
            query = isDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
        return await PagedList<T>.CreateAsync(query, pageNumber, pageSize);
    }

    public async Task<PagedList<T>> GetPagedWithIncludesAsync(
        int pageNumber,
        int pageSize,
        Func<IQueryable<T>, IQueryable<T>>? includes = null,
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>? orderBy = null,
        bool isDescending = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbSet;
        if (includes != null)
            query = includes(query);
        if (predicate != null)
            query = query.Where(predicate);
        if (orderBy != null)
            query = isDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
        return await PagedList<T>.CreateAsync(query, pageNumber, pageSize);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(predicate, cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
```

**Cách dùng trong Service:**
```csharp
// Phân trang + sắp xếp
var result = await _repository.GetPagedWithOrderAsync(
    pageNumber: 1,
    pageSize: 10,
    predicate: x => x.Status == OrderStatus.Active,
    orderBy: x => x.CreatedAt,
    isDescending: true
);

// Phân trang + Include + sắp xếp
var result = await _repository.GetPagedWithIncludesAsync(
    pageNumber: 1,
    pageSize: 10,
    includes: q => q.Include(x => x.User).Include(x => x.Product),
    predicate: x => x.Status == OrderStatus.Active,
    orderBy: x => x.CreatedAt,
    isDescending: true
);
```

#### 7. Controller Return Type

| Loại API | Kiểu trả về | Method dùng |
|----------|-------------|--------------|
| CRUD (Create/Update/Delete) | `IActionResult` | `Ok(data, message)` |
| GET single by id | `IActionResult` | `Ok(data, message)` |
| GET paged list | `IActionResult` | `OkPaged(pagedData, message)` |

**Mẫu code:**
```csharp
// Create
[HttpPost]
public async Task<IActionResult> Create(CreateDto request)
{
    var result = await _service.CreateAsync(request);
    return Ok(result, _localizer["SuccessMessage"]);
}

// Get paged
[HttpGet]
public async Task<IActionResult> GetList([FromQuery] int page = 1, int size = 10)
{
    var result = await _service.GetPagedAsync(page, size);
    return OkPaged(result, _localizer["ListRetrievedSuccess"]);
}
```

#### 8. Response Classes & Paging

**ApiResponse<T>** - Cho single object (CRUD):
```csharp
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
    public DateTime Timestamp { get; set; }
}
```

**PagedList<T>** - Cho dữ liệu phân trang (Application Layer):
- Namespace: `FlashOffer.API.Domain.Models`
- Dùng trong **Service** để nhận từ Repository và trả về Controller
- Chứa: `Items`, `PageNumber`, `TotalPages`, `TotalCount`, `HasPreviousPage`, `HasNextPage`

```csharp
public class PagedList<T>
{
    public List<T> Items { get; }
    public int PageNumber { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}
```

**PagedResponse<T>** - Cho response phân trang (WebApi Layer):
- Namespace: `FlashOffer.API.WebApi.Responses`
- Dùng trong **Controller** để wrap dữ liệu thành response JSON

```csharp
public class PagedResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<T> Data { get; set; } = new();
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
    public DateTime Timestamp { get; set; }

    public static PagedResponse<T> Ok(PagedList<T> pagedData, string message = "Success")
    {
        return new PagedResponse<T>
        {
            Success = true,
            Message = message,
            Data = pagedData.Items,
            PageNumber = pagedData.PageNumber,
            TotalPages = pagedData.TotalPages,
            TotalCount = pagedData.TotalCount,
            HasPreviousPage = pagedData.HasPreviousPage,
            HasNextPage = pagedData.HasNextPage,
            Timestamp = DateTime.UtcNow
        };
    }
}
```

**ApiControllerBase methods:**

| Method | Input | Output |
|--------|-------|--------|
| `Ok<T>(T data, string message)` | Single object | `ApiResponse<T>` |
| `OkPaged<T>(PagedList<T> pagedData, string message)` | `PagedList<T>` | `PagedResponse<T>` |
| `BadRequest(string message, List<string> errors)` | Error info | `ApiResponse<object>` |
| `NotFound(string message)` | Message | `ApiResponse<object>` |

**Mẫu code:**
```csharp
// Service - trả về PagedList
public async Task<PagedList<ResponseDto>> GetPagedAsync(QueryDto query)
{
    var pagedEntities = await _repository.GetPagedWithOrderAsync(
        query.Page, 
        query.PageSize, 
        predicate, 
        x => x.CreatedAt, 
        true
    );
    var items = _mapper.Map<List<ResponseDto>>(pagedEntities.Items);
    return new PagedList<ResponseDto>(items, pagedEntities.TotalCount, query.Page, query.PageSize);
}

// Controller - dùng OkPaged
[HttpGet]
public async Task<IActionResult> GetList([FromQuery] QueryDto query)
{
    var result = await _service.GetPagedAsync(query);
    return OkPaged(result, _localizer["ListRetrievedSuccess"]);
}
```

**Lưu ý:** 
- `PagedList<T>` dùng trong Service (tầng Application)
- `PagedResponse<T>` dùng trong Controller (tầng WebApi)
- **Không dùng `PagedResultDto<T>`** (đã có `PagedList<T>` thay thế)

### Thêm API mới - Quy trình 10 bước

| Bước | Hành động | Thư mục | Resource keys |
|------|-----------|---------|---------------|
| 1 | Tạo Entity (dùng Enum nếu cần) | `Domain/Entities/` | - |
| 2 | Tạo EF Configuration | `Infrastructure/Data/Configurations/` | - |
| 3 | Tạo DTOs + IMapFrom | `Application/DTOs/` | - |
| 4 | Tạo Validator (+ IStringLocalizer) | `Application/Validators/` | - |
| 5 | Thêm resource keys (CHỈ key mới) | `Application/Resources/` | **GỬI NGAY** en + vi |
| 6 | Thêm DbSet | `Infrastructure/Data/AppDbContext.cs` | - |
| 7 | Tạo Service/Command + Handler | `Application/Services/` hoặc `Application/Features/` | - |
| 8 | Đăng ký Service/MediatR trong DI | `Application/DependencyInjection.cs` | - |
| 9 | Tạo Controller (dùng ApiControllerBase) | `WebApi/Controllers/` | - |
| 10 | Chạy migration | Terminal | - |

### Quy tắc xử lý Issue API

| Bước | Hành động | Ví dụ |
|------|-----------|-------|
| 1 | **Hỏi Entity đã có chưa?** | "Entity PurchaseRequest đã có chưa? Nếu có, gửi tôi code hiện tại." |
| 2 | **Hỏi DTO/Request/Response đã có chưa?** | "DTOs đã tạo chưa? Cần request/response nào?" |
| 3 | **Đợi người dùng gửi code hiện có** | Không tự ý tạo mới nếu đã có |
| 4 | **Phân tích và đề xuất bổ sung** | Nếu đã có: đề xuất thêm field, validation, mapping |
| 5 | **Confirm trước khi code** | Hỏi: "Tôi đề xuất thêm X, Y. Bạn đồng ý không?" |
| 6 | **Thực hiện các bước còn lại** | Chỉ code các phần chưa có |

### Quy tắc Migration (QUAN TRỌNG)

**Tạo migration:**
```bash
# Từ thư mục Infrastructure
dotnet ef migrations add [MigrationName] --startup-project ../FlashOffer.API.WebApi

# Hoặc từ thư mục gốc
dotnet ef migrations add [MigrationName] --project src/FlashOffer.API.Infrastructure --startup-project src/FlashOffer.API.WebApi --output-dir Data/Migrations
```

**Cập nhật database:**
```bash
dotnet ef database update --project src/FlashOffer.API.Infrastructure --startup-project src/FlashOffer.API.WebApi
```

**Xóa migration:**
```bash
dotnet ef migrations remove --project src/FlashOffer.API.Infrastructure --startup-project src/FlashOffer.API.WebApi
```

**Lưu ý:** Migration name nên có ý nghĩa: `Add[TableName]Table`, `Update[FieldName]`, v.v.

### Quy tắc Resource Keys (QUAN TRỌNG)

**Khi đến Bước 5, phải gửi NGAY 2 file resource (en + vi) với CHỈ các key mới phát sinh.**

**Không liệt kê lại các key đã có sẵn (ProductNameRequired, PhoneInvalid...).**

### Quy tắc Testing (QUAN TRỌNG)

#### Unit Test (kiểm thử đơn vị)
- **Mục đích:** Test một đơn vị code nhỏ trong isolation
- **Đặc điểm:** Mock tất cả dependencies, không chạy database/API thật
- **Tốc độ:** Rất nhanh (ms)

| Thành phần | Unit Test |
|------------|-----------|
| Validator | ✅ Test validation rules |
| Handler/Service | ✅ Test business logic |
| DTO mapping | ✅ Test AutoMapper |

**Lưu ý khi viết Unit Test với Moq:**
- Methods có optional parameters (CancellationToken cancellationToken = default) phải truyền đủ số lượng tham số với `It.IsAny<T>()`

```csharp
// ✅ ĐÚNG
_repositoryMock.Setup(r => r.AddAsync(It.IsAny<CtvRegistration>(), It.IsAny<CancellationToken>()))
    .Returns(Task.CompletedTask);
_repositoryMock.Verify(r => r.AddAsync(It.IsAny<CtvRegistration>(), It.IsAny<CancellationToken>()), Times.Once);

// ❌ SAI - Thiếu CancellationToken
_repositoryMock.Setup(r => r.AddAsync(It.IsAny<CtvRegistration>())).Returns(Task.CompletedTask);
_repositoryMock.Verify(r => r.AddAsync(It.IsAny<CtvRegistration>()), Times.Once);
```

#### Validator Test (kiểm thử validation rules)

- **Mục đích:** Test các validation rules của DTO
- **Đặc điểm:** Khởi tạo validator trực tiếp, không dùng Service/Mock
- **Yêu cầu:** Mock `IStringLocalizer<SharedResource>` khi validator có inject localizer

**Mẫu code:**
```csharp
public class CreateXxxValidatorTests
{
    private readonly Mock<IStringLocalizer<SharedResource>> _localizerMock;
    private readonly CreateXxxValidator _validator;

    public CreateXxxValidatorTests()
    {
        _localizerMock = new Mock<IStringLocalizer<SharedResource>>();
        _validator = new CreateXxxValidator(_localizerMock.Object);
    }

    [Fact]
    public void Validate_ValidDto_ShouldSucceed()
    {
        var dto = new CreateXxxDto { /* fields */ };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_FieldInvalid_ShouldFail()
    {
        var dto = new CreateXxxDto { /* invalid field */ };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "FieldName");
    }
}
```

**Lưu ý khi viết Validator Test với IStringLocalizer:**

- Phải setup **tất cả resource keys** mà validator sử dụng trong `WithMessage()`.
- Thiếu setup sẽ gây `ArgumentNullException` khi validator khởi tạo.
- Cách xác định keys cần setup: Đọc file validator, tìm tất cả `localizer["KeyName"]`.

**Mẫu setup đầy đủ:**
```csharp
_localizerMock.Setup(l => l["ProductNameRequired"])
    .Returns(new LocalizedString("ProductNameRequired", "Product name is required"));
_localizerMock.Setup(l => l["TargetPeopleCountMin"])
    .Returns(new LocalizedString("TargetPeopleCountMin", "Target people count must be at least 2"));
```

**Lưu ý:** Không nhầm lẫn Validator Test với Service Test. Validator test chỉ test validation logic, không test business logic.

**Phân biệt tên file:**

| Loại test | Tên file | Nội dung test |
|-----------|----------|---------------|
| Service Test | `{Feature}ServiceTests.cs` | Business logic, repository mock |
| Validator Test | `{Feature}ValidatorTests.cs` | Validation rules, không dùng repository |
| Controller Test | `{Feature}ControllerTests.cs` | HTTP response, integration |

#### Quy tắc viết test mới (BẮT BUỘC)

**Khi được yêu cầu viết test cho một feature mới, phải tuân thủ quy trình sau:**

| Bước | Hành động | Ví dụ |
|-------|-----------|-------|
| 1 | **Hỏi loại test** cần viết (Service, Handler, Validator, Integration) | "Bạn muốn viết test cho Service hay Validator?" |
| 2 | **Hỏi thông tin DTO** (các field, validation rules) | "DTO `CreateXxxDto` có những field nào? Ràng buộc gì?" |
| 3 | **Hỏi dependencies** (repository, mapper, localizer...) | "Validator có inject `IStringLocalizer` không?" |
| 4 | **Hỏi business logic** cần kiểm tra | "Khi tạo mới, field nào được set mặc định?" |
| 5 | **SAU KHI có đầy đủ thông tin** mới viết code | - |

**Nghiêm cấm:**
- Tự đoán field của DTO
- Tự đoán validator có localizer hay không
- Viết test khi chưa confirm thông tin với người dùng
- Áp dụng template cũ khi chưa kiểm tra cấu trúc hiện tại

**Mẫu câu hỏi khi bắt đầu viết test:**
```
"Tôi cần các thông tin sau để viết test:
1. DTO cần test có những field nào?
2. Validator có inject IStringLocalizer không?
3. Có business logic đặc biệt nào cần kiểm tra không?"
```

#### Integration Test (kiểm thử tích hợp)
- **Mục đích:** Test nhiều thành phần hoạt động cùng nhau
- **Đặc điểm:** Dùng database thật (InMemory/TestContainer), gọi API endpoint
- **Tốc độ:** Chậm hơn (giây)

| Thành phần | Integration Test |
|------------|------------------|
| Controller | ✅ Test HTTP response |
| Repository | ✅ Test CRUD với database |
| API flow | ✅ Test request → response |

**BaseIntegrationTest Pattern (BẮT BUỘC):**
```csharp
// BaseIntegrationTest phải xóa sạch DbContext registrations
services.RemoveAll(typeof(ApplicationDbContext));
services.RemoveAll(typeof(IApplicationDbContext));
services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));

// Sau đó mới thêm InMemory - tạo dbName trước lambda
var dbName = $"TestDb_{Guid.NewGuid()}";
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase(dbName));
services.AddScoped<IApplicationDbContext>(sp => 
    sp.GetRequiredService<ApplicationDbContext>());
```

**Lưu log Integration Test ra file (KHI CẦN DEBUG):**
```bash
dotnet test tests/FlashOffer.API.IntegrationTests --logger "console;verbosity=detailed" > tests/FlashOffer.API.IntegrationTests/logs/test_output.log 2>&1
```

#### Quy tắc cho dự án FlashOffer

| Loại test | Khi nào viết | Thư mục | Cần Base class? |
|-----------|--------------|---------|-----------------|
| Unit Test | Mỗi Validator, Handler, Service | `tests/FlashOffer.API.UnitTests/` | ❌ Không |
| Integration Test | Mỗi Controller (1 file chính) | `tests/FlashOffer.API.IntegrationTests/` | ✅ Cần `BaseIntegrationTest` |

**Lệnh chạy test với log:**

| Loại test | Lệnh |
|-----------|------|
| Unit Test | `dotnet test tests/FlashOffer.API.UnitTests/ --logger "console;verbosity=detailed" > tests/FlashOffer.API.UnitTests/logs/test_output.log 2>&1` |
| Integration Test | `dotnet test tests/FlashOffer.API.IntegrationTests/ --logger "console;verbosity=detailed" > tests/FlashOffer.API.IntegrationTests/logs/test_output.log 2>&1` |
| Tất cả tests | `dotnet test --logger "console;verbosity=detailed" > tests/logs/test_output.log 2>&1` |

**Tạo thư mục logs trước khi chạy:**
```bash
mkdir -p tests/FlashOffer.API.UnitTests/logs
mkdir -p tests/FlashOffer.API.IntegrationTests/logs
mkdir -p tests/logs
```

**Lưu ý:** Unit Test luôn mock `IRepository<T>`, không dùng database thật.

### Lưu ý quan trọng

- **Repository.AddAsync** cần `SaveChangesAsync()` sau đó
- Logic nghiệp vụ đặt trong Service/Handler, không trong Controller
- **BẮT BUỘC** cấu hình `SuppressModelStateInvalidFilter = true`
- **Mọi message client** đều qua `IStringLocalizer`
- **Resource keys:** chỉ thêm key mới, KHÔNG liệt kê key đã tồn tại
- **Enum:** ưu tiên dùng thay vì string, cấu hình `HasConversion<int>()`
- **Phone validation:**