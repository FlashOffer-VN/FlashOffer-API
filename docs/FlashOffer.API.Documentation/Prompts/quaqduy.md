# 📋 FLASHOFFER - QUY TẮC DỰ ÁN (FULL)

---

## 1. 🧩 Behaviors (MediatR Pipeline)

### Vị trí:
`src/FlashOffer.API.Application/Common/Behaviors/`

### Các Behavior có sẵn:

| Behavior | File | Mục đích |
|----------|------|----------|
| `LoggingBehavior<TRequest, TResponse>` | `LoggingBehavior.cs` | Log mọi request/response |
| `ValidationBehavior<TRequest, TResponse>` | `ValidationBehavior.cs` | Tự động validate request qua FluentValidation |
| `PerformanceBehavior<TRequest, TResponse>` | `PerformanceBehavior.cs` | Đo hiệu năng, cảnh báo request chậm (>500ms) |
| `TransactionBehavior<TRequest, TResponse>` | `TransactionBehavior.cs` | Quản lý transaction cho Command |

### Quy tắc sử dụng:

| STT | Quy tắc | Mô tả |
|-----|---------|-------|
| 1 | **Đăng ký theo thứ tự** | `LoggingBehavior` → `ValidationBehavior` → `PerformanceBehavior` → `TransactionBehavior` |
| 2 | **Interface marker** | Command/Request implement `ITransactionalRequest` để dùng transaction |
| 3 | **Validator tự động** | Tạo Validator class, Behavior tự động gọi |

### Đăng ký trong DI:
```csharp
services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
});
```

---

## 2. 🚨 Exceptions

### Vị trí:
`src/FlashOffer.API.Shared/Exceptions/`

### Custom Exceptions:

| Exception | File | HTTP Status | Khi nào dùng |
|-----------|------|-------------|--------------|
| `NotFoundException` | `NotFoundException.cs` | 404 | Không tìm thấy dữ liệu |
| `BadRequestException` | `BadRequestException.cs` | 400 | Request không hợp lệ |
| `ConflictException` | `ConflictException.cs` | 409 | Dữ liệu bị trùng lặp |
| `ForbiddenException` | `ForbiddenException.cs` | 403 | Không có quyền truy cập |
| `UnauthorizedException` | `UnauthorizedException.cs` | 401 | Chưa xác thực |
| `ValidationException` | `ValidationException.cs` | 400 | Lỗi validation từ FluentValidation |

### Quy tắc sử dụng:

| STT | Quy tắc | Ví dụ |
|-----|---------|-------|
| 1 | **Luôn dùng IStringLocalizer** | `throw new NotFoundException(_localizer["User_NotFound"]);` |
| 2 | **Resource key có prefix** | `Collaborator_NotFound`, `PurchaseRequest_InvalidStatus` |
| 3 | **Không hardcode message** | ❌ `throw new Exception("Không tìm thấy")` |

### Code mẫu:
```csharp
// ✅ Đúng
throw new NotFoundException(_localizer["Collaborator_NotFound"]);

// ❌ Sai
throw new Exception("Không tìm thấy cộng tác viên");
```

---

## 3. 🗄️ UnitOfWork + Repository (Auto-Save)

### Vị trí:

| File | Đường dẫn |
|------|-----------|
| `IUnitOfWork` | `Domain/Interfaces/IUnitOfWork.cs` |
| `UnitOfWork` | `Infrastructure/Data/UnitOfWork.cs` |
| `GenericRepository` | `Infrastructure/Repositories/GenericRepository.cs` |

### Quy tắc Auto-Save:

| STT | Quy tắc | Mô tả |
|-----|---------|-------|
| 1 | **Tự động SaveChanges** | `AddAsync`/`UpdateAsync`/`DeleteAsync` tự động gọi `SaveChanges` |
| 2 | **Có cả Sync và Async** | Hỗ trợ cả `AddAsync` và `Add` (sync) |
| 3 | **Transaction qua Behavior** | Dùng `ITransactionalRequest` để bật transaction |
| 4 | **Không gọi SaveChanges thủ công** | Trừ trường hợp đặc biệt, không cần gọi trong Service |

### Code mẫu trong Service:
```csharp
// ✅ Tự động save - Không cần SaveChangesAsync()
public async Task CreateAsync(CreateDto request)
{
    var entity = _mapper.Map<Collaborator>(request);
    await _repository.AddAsync(entity); // Tự động save
}

// ❌ Sai - Không cần gọi SaveChanges thủ công
public async Task CreateAsync(CreateDto request)
{
    var entity = _mapper.Map<Collaborator>(request);
    await _repository.AddAsync(entity);
    await _repository.SaveChangesAsync(); // ❌ Thừa
}
```

---

## 4. 🎯 Controller & API Rules

### Controller Base:

```csharp
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public abstract class ApiControllerBase : ControllerBase
{
    // Base class for all controllers
}
```

### Quy tắc API:

| STT | Quy tắc | Mô tả |
|-----|---------|-------|
| 1 | **Kế thừa ApiControllerBase** | Tất cả controller kế thừa `ApiControllerBase` |
| 2 | **Dùng ApiVersion** | URL format: `/api/v1/[controller]` |
| 3 | **Return IActionResult** | Dùng `Ok()`, `BadRequest()`, `NotFound()` |
| 4 | **Response chuẩn** | Dùng `ApiResponse<T>` và `PagedResponse<T>` |

### Method dùng:

| Loại API | Method | Ví dụ |
|----------|--------|-------|
| GET single | `Ok(data)` | `return Ok(collaborator);` |
| GET list (paged) | `OkPaged(pagedData)` | `return OkPaged(result);` |
| Create/Update | `Ok(data, message)` | `return Ok(collaborator, "Tạo thành công");` |
| Delete | `Ok(message)` | `return Ok("Xóa thành công");` |
| Error | `BadRequest(message)` | `return BadRequest("Lỗi validation");` |

### Controller mẫu:
```csharp
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/collaborators")]
[Authorize]
public class CollaboratorController : ApiControllerBase
{
    private readonly ICollaboratorService _service;

    public CollaboratorController(ICollaboratorService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCollaboratorDto request)
    {
        var result = await _service.CreateAsync(request);
        return Ok(result, _localizer["Collaborator_CreateSuccess"]);
    }
}
```

---

## 5. 📋 QUY TẮC PHÁT TRIỂN CHI TIẾT

### 5.1 DTOs & Mapping (AutoMapper) - BẮT BUỘC

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

### 5.2 Validation (FluentValidation)

- Inject `IStringLocalizer<SharedResource>` cho message đa ngôn ngữ
- Dùng resource key, không hardcode message

**Phone validation (tránh lỗi trùng lặp):**

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

### 5.3 Resource Keys - QUY TẮC PREFIX (BẮT BUỘC)

**Nguyên tắc đặt tên key:**
- **Tất cả resource keys đều phải có prefix theo tên Feature/Entity**
- Format: `{FeatureName}_{KeyName}`
- Ví dụ: `PurchaseRequest_ProductNameRequired`, `Order_StatusPending`

**Lý do:** Tránh xung đột key giữa các feature, dễ dàng quản lý và tìm kiếm, phân biệt rõ key thuộc feature nào.

| Loại | Format | Ví dụ |
|------|--------|-------|
| Success | `{Feature}_{Action}Success` | `PurchaseRequest_CreateSuccess` |
| Not Found | `{Feature}_NotFound` | `PurchaseRequest_NotFound` |
| Validation | `{Feature}_{FieldName}Required` | `PurchaseRequest_ProductNameRequired` |
| Validation | `{Feature}_{FieldName}Invalid` | `PurchaseRequest_PhoneInvalid` |
| Validation | `{Feature}_{FieldName}MinLength` | `PurchaseRequest_ProductNameMinLength` |
| Export Title | `Export{Feature}Title` | `ExportPurchaseRequestsTitle` |
| Export Header | `Export{Feature}_{FieldName}` | `ExportPurchaseRequests_ProductName` |

**Ví dụ cụ thể cho PurchaseRequest:**
```xml
<!-- Validation Keys -->
<data name="PurchaseRequest_ProductNameRequired"><value>Tên sản phẩm là bắt buộc</value></data>
<data name="PurchaseRequest_PhoneInvalid"><value>Số điện thoại không hợp lệ</value></data>

<!-- Success Keys -->
<data name="PurchaseRequest_CreateSuccess"><value>Tạo yêu cầu thành công</value></data>
```

**Quy tắc bổ sung:**
- Chỉ thêm key mới khi chưa tồn tại trong hệ thống
- Key cũ (không prefix) vẫn giữ nguyên để không break các feature đã có
- Khi tạo key mới cho feature, **bắt buộc** phải dùng prefix
- Prefix phải trùng tên Feature/Entity

### 5.4 Enum

- Đặt trong `Domain/Enums/`
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

### 5.5 Service Layer - 2 cách tiếp cận

| Cách | Đường dẫn | Phù hợp |
|------|-----------|---------|
| Service trực tiếp | `I{Feature}Service` / `{Feature}Service` | CRUD đơn giản |
| MediatR CQRS | `Features/{Feature}/Commands|Queries|Handlers` | Logic phức tạp |

**Quy tắc chọn:**

| Tiêu chí | Service | MediatR |
|----------|---------|---------|
| CRUD đơn giản | ✅ | ❌ |
| Cần cross-cutting concerns | ❌ | ✅ |
| Số lượng method | 1-3 | >5 |
| Độ phức tạp | Thấp | Cao |

### 5.6 Repository Methods

| Method | Mô tả |
|--------|-------|
| `GetByIdAsync(Guid id)` | Lấy entity theo Id (tự động filter IsDeleted) |
| `GetFirstAsync(predicate)` | Lấy entity đầu tiên thỏa điều kiện |
| `GetAllAsync()` | Lấy tất cả entity (chưa xóa) |
| `FindAsync(predicate)` | Lấy danh sách thỏa điều kiện |
| `GetPagedAsync(page, size, predicate)` | Phân trang cơ bản |
| `GetPagedWithOrderAsync(page, size, predicate, orderBy, isDescending)` | Phân trang + sắp xếp |
| `GetPagedWithIncludesAsync(page, size, includes, predicate, orderBy, isDescending)` | Phân trang + Include navigation |
| `GetFirstWithIncludesAsync(predicate, includes)` | Lấy 1 entity kèm Include |
| `GetListWithIncludesAsync(includes, predicate, orderBy, isDescending)` | Lấy danh sách kèm Include (không phân trang) |
| `CountAsync(predicate)` | Đếm số lượng bản ghi |
| `AnyAsync(predicate)` | Kiểm tra tồn tại |
| `FromSqlRawAsync(sql, parameters)` | Thực thi SQL raw (báo cáo phức tạp) |
| `GetDeletedAsync()` | Lấy danh sách đã xóa mềm |
| `AddAsync(entity)` | Thêm mới 1 entity |
| `AddRangeAsync(entities)` | Thêm mới nhiều entity |
| `Update(entity)` | Cập nhật 1 entity |
| `UpdateRange(entities)` | Cập nhật nhiều entity |
| `Delete(entity)` | Xóa mềm (set IsDeleted = true) |
| `DeleteRange(entities)` | Xóa mềm nhiều entity |
| `Restore(entity)` | Khôi phục soft delete |
| `RestoreRange(entities)` | Khôi phục nhiều entity |
| `SaveChangesAsync()` | Lưu thay đổi vào database |

### 5.7 Controller Return Type

| Loại API | Kiểu trả về | Method dùng |
|----------|-------------|--------------|
| CRUD (Create/Update/Delete) | `IActionResult` | `Ok(data, message)` |
| GET single by id | `IActionResult` | `Ok(data, message)` |
| GET paged list | `IActionResult` | `OkPaged(pagedData, message)` |

### 5.8 Response Classes & Paging

**ApiResponse<T>** - Cho single object:
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

**PagedList<T>** - Application Layer: `FlashOffer.API.Domain.Models`
**PagedResponse<T>** - WebApi Layer: `FlashOffer.API.WebApi.Responses`

### 5.9 Export Excel (EPPlus) - QUAN TRỌNG

**Vị trí:**
- Interface: `Application/Common/Interfaces/IExcelService.cs`
- Implementation: `Infrastructure/Services/ExcelService.cs`
- License: EPPlus 7.x dùng `ExcelPackage.LicenseContext = LicenseContext.NonCommercial` trong Program.cs

**Quy tắc viết Export Handler:**
1. Inject dependencies: `IRepository<T>`, `IExcelService`, `IStringLocalizer<SharedResource>`
2. Column config dùng resource key: `["Export{Feature}_{FieldName}"] = x => x.Property`
3. Gọi `_excelService.ExportToExcel(data, columns, "FeatureName", "Export{Feature}Title", _localizer)`
4. Predicate dùng `ExpressionExtensions.AndAlso`
5. Controller trả về `File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName)`

**ExcelService format tự động:**
- DateTime → `yyyy-MM-dd HH:mm:ss`
- Số → `#,##0`
- Header: căn trái, bold, background gray
- Data: căn trái, có border

### 5.10 Soft Delete & Global Query Filter

**Quy tắc Soft Delete:**
- Tất cả Entity kế thừa `BaseEntity` đều có `IsDeleted` flag.
- Method `Delete()` và `DeleteRange()` chỉ set `IsDeleted = true`, **không xóa vật lý**.
- Method `Restore()` và `RestoreRange()` để khôi phục dữ liệu đã xóa mềm.

**Global Query Filter:**
- Trong `ApplicationDbContext.OnModelCreating()`, tự động thêm filter `IsDeleted = false` cho tất cả entity kế thừa `BaseEntity`.
- Đảm bảo mọi query đều chỉ lấy dữ liệu chưa xóa.

```csharp
// ApplicationDbContext.cs
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    
    foreach (var entityType in modelBuilder.Model.GetEntityTypes())
    {
        if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
        {
            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var property = Expression.Property(parameter, "IsDeleted");
            var condition = Expression.Equal(property, Expression.Constant(false));
            var lambda = Expression.Lambda(condition, parameter);
            entityType.SetQueryFilter(lambda);
        }
    }
}
```

### 5.11 Expression Extensions - Gộp predicate (BẮT BUỘC)

**Vị trí:** `src/FlashOffer.API.Shared/Extensions/ExpressionExtensions.cs`

**Các method:**
| Method | Công dụng |
|--------|-----------|
| `.And()` | Gộp 2 điều kiện với `&&` |
| `.Or()` | Gộp 2 điều kiện với `\|\|` |
| `.AndAlso()` | Tương tự `.And()` |

**Sử dụng trong Service:**
```csharp
using FlashOffer.API.Shared.Extensions;

Expression<Func<Entity, bool>>? predicate = null;

// Gộp điều kiện dần
predicate = predicate.And(p => p.Status == Status.Active);
predicate = predicate.And(p => p.CreatedAt >= startDate);
predicate = predicate.Or(p => p.Priority == Priority.High);

// Kết quả: (Status == Active && CreatedAt >= startDate) || Priority == High
```

**Quy tắc:**
- **BẮT BUỘC** dùng `ExpressionExtensions.And()` thay vì tự viết `CombinePredicates` trong Service
- Xóa method `CombinePredicates` và `ReplaceExpressionVisitor` khỏi Service khi đã có extension này
- `predicate` khởi tạo = `null`, sau đó gọi `.And()` hoặc `.Or()` để gộp dần

**Code mẫu trong Service:**
```csharp
public async Task<PagedList<PostResponse>> GetPostsAsync(GetPostsQuery query)
{
    Expression<Func<SocialPost, bool>>? predicate = null;

    if (query.Type.HasValue)
        predicate = predicate.And(p => p.Type == query.Type.Value);
    
    if (!string.IsNullOrEmpty(query.Tag))
        predicate = predicate.And(p => p.PostTags.Any(pt => pt.Tag.Name == query.Tag));
    
    if (!isAdmin)
        predicate = predicate.And(p => p.Privacy == PrivacyType.Public);

    predicate ??= p => true;  // Nếu không có filter, lấy tất cả

    var posts = await _repository.GetPagedWithIncludesAsync(...);
    // ...
}
```

### 5.12 Queryable Extensions - Include linh hoạt (BẮT BUỘC)

**Vị trí:** `src/FlashOffer.API.Shared/Extensions/QueryableExtensions.cs`

**Các method:**

| Method | Công dụng |
|--------|-----------|
| `IncludeMultiple<T>()` | Include nhiều navigation cùng lúc |
| `IncludeThen<T, TProperty, TThen>()` | Include + ThenInclude |

**Code:**
```csharp
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FlashOffer.API.Shared.Extensions;

public static class QueryableExtensions
{
    /// <summary>
    /// Include nhiều navigation properties cùng lúc
    /// </summary>
    public static IQueryable<T> IncludeMultiple<T>(
        this IQueryable<T> query,
        params Expression<Func<T, object>>[] includes)
        where T : class
    {
        if (includes == null || includes.Length == 0)
            return query;

        var result = query;
        foreach (var include in includes)
        {
            result = result.Include(include);
        }
        return result;
    }

    /// <summary>
    /// Include + ThenInclude với cú pháp ngắn
    /// </summary>
    public static IQueryable<T> IncludeThen<T, TProperty, TThen>(
        this IQueryable<T> query,
        Expression<Func<T, TProperty>> include,
        Expression<Func<TProperty, TThen>> thenInclude)
        where T : class
    {
        return query.Include(include).ThenInclude(thenInclude);
    }
}
```

**Sử dụng trong Service:**
```csharp
// Cách 1 - Include nhiều
var posts = await _repository.GetPagedWithIncludesAsync(
    includes: q => q.IncludeMultiple(
        p => p.Author,
        p => p.PostTags
    ),
    // ...
);

// Cách 2 - Include + ThenInclude
var posts = await _repository.GetPagedWithIncludesAsync(
    includes: q => q.IncludeThen(
        p => p.PostTags,
        pt => pt.Tag
    ),
    // ...
);

// Cách 3 - Kết hợp native (khi cần nhiều ThenInclude)
var posts = await _repository.GetPagedWithIncludesAsync(
    includes: q => q
        .Include(p => p.Author)
        .Include(p => p.PostTags)
            .ThenInclude(pt => pt.Tag),
    // ...
);
```

**Quy tắc:**
- **Ưu tiên dùng `IncludeMultiple()`** cho các include đơn giản, không có ThenInclude
- **Dùng native Include + ThenInclude** khi cần nhiều cấp ThenInclude
- **Không tạo method IncludeSocialDetails** cứng cho từng Entity - dùng generic để tái sử dụng
- Thêm `using Microsoft.EntityFrameworkCore;` cho file extension

### 5.13 Xử lý User trong các API

**Nguyên tắc chung:**
- Mọi API tạo dữ liệu (Create) đều cần gán `UserId` từ token hiện tại hoặc tạo User ngầm
- API lấy danh sách (GetList) cho User chỉ lấy dữ liệu của user đó
- API lấy danh sách (GetList) cho Admin lấy tất cả dữ liệu

**Quy tắc cụ thể:**

| Loại API | UserId lấy từ | Hành động |
|----------|---------------|-----------|
| Create (Public - chưa login) | Tự động tạo User | Tạo User ngầm (nếu chưa có) dựa trên Phone/Email |
| Create (Auth - đã login) | `ICurrentUserService.UserId` | Gán trực tiếp |
| GetList (User thường) | `ICurrentUserService.UserId` | Filter theo UserId |
| GetList (Admin) | Không filter | Lấy tất cả |

**Code mẫu cho Create API (Service/Handler):**
```csharp
// 1. Lấy UserId từ token (nếu có)
var userId = _currentUserService.UserId;

// 2. Nếu là Public API (chưa đăng nhập), tạo User ngầm
if (string.IsNullOrEmpty(userId))
{
    userId = await _userService.GetOrCreateUserAsync(
        request.FullName, 
        request.Phone, 
        request.Email
    );
}

// 3. Gán vào entity
var entity = _mapper.Map<TEntity>(request);
entity.UserId = userId;
```

**Code mẫu cho GetList API:**
```csharp
// Admin - lấy tất cả
if (_currentUserService.IsInRole("Admin"))
{
    var result = await _repository.GetPagedAsync(query);
}
// User - chỉ lấy của mình
else
{
    var userId = _currentUserService.UserId;
    var result = await _repository.GetPagedAsync(query, 
        x => x.UserId == userId);
}
```

**Interface ICurrentUserService:**
```csharp
public interface ICurrentUserService
{
    string? UserId { get; }
    string? UserName { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(string role);
}
```

**Service lấy/tạo User:**
```csharp
public interface IUserService
{
    Task<Guid> GetOrCreateUserAsync(string fullName, string phone, string? email = null);
    Task<User?> GetCurrentUserAsync();
}
```

**Trong Controller:**
```csharp
// Sử dụng ICurrentUserService
[Authorize]
[HttpPost("my-data")]
public async Task<IActionResult> CreateMyData([FromBody] CreateDto request)
{
    var userId = _currentUserService.UserId;
    // ... logic
}
```

---

## 6. 📝 THÊM API MỚI - QUY TRÌNH 10 BƯỚC

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

---

## 7. 🔍 QUY TRÌNH XỬ LÝ ISSUE API

| Bước | Hành động | Ví dụ |
|------|-----------|-------|
| 1 | **Hỏi Entity đã có chưa?** | "Entity PurchaseRequest đã có chưa? Nếu có, gửi tôi code hiện tại." |
| 2 | **Hỏi DTO/Request/Response đã có chưa?** | "DTOs đã tạo chưa? Cần request/response nào?" |
| 3 | **Đợi người dùng gửi code hiện có** | Không tự ý tạo mới nếu đã có |
| 4 | **Phân tích và đề xuất bổ sung** | Nếu đã có: đề xuất thêm field, validation, mapping |
| 5 | **Confirm trước khi code** | Hỏi: "Tôi đề xuất thêm X, Y. Bạn đồng ý không?" |
| 6 | **Thực hiện các bước còn lại** | Chỉ code các phần chưa có |

---

## 8. 🗄️ MIGRATION - QUY TẮC

**Vị trí migrations:** `src/FlashOffer.API.Infrastructure/Data/Migrations/`

**Luôn sử dụng `--output-dir Data/Migrations` cho mọi lệnh migration:**

```bash
# Tạo migration
dotnet ef migrations add [MigrationName] --project src/FlashOffer.API.Infrastructure --startup-project src/FlashOffer.API.WebApi --output-dir Data/Migrations

# Cập nhật database
dotnet ef database update --project src/FlashOffer.API.Infrastructure --startup-project src/FlashOffer.API.WebApi

# Xóa migration cuối
dotnet ef migrations remove --project src/FlashOffer.API.Infrastructure --startup-project src/FlashOffer.API.WebApi

# Rollback về migration cụ thể
dotnet ef database update [MigrationName] --project src/FlashOffer.API.Infrastructure --startup-project src/FlashOffer.API.WebApi
```

**Xóa Database - BẮT BUỘC HỎI:**
```bash
# Chỉ khi được xác nhận mới chạy
dotnet ef database drop --project src/FlashOffer.API.Infrastructure --startup-project src/FlashOffer.API.WebApi
```

**Migration Checklist:**

| Bước | Hành động | Lệnh |
|------|-----------|------|
| 1 | Tạo migration | `dotnet ef migrations add [Name] --project src/FlashOffer.API.Infrastructure --startup-project src/FlashOffer.API.WebApi --output-dir Data/Migrations` |
| 2 | Áp dụng migration | `dotnet ef database update --project src/FlashOffer.API.Infrastructure --startup-project src/FlashOffer.API.WebApi` |
| 3 | Xóa migration | `dotnet ef migrations remove --project src/FlashOffer.API.Infrastructure --startup-project src/FlashOffer.API.WebApi` |
| 4 | Xóa database | **HỎI TRƯỚC**, sau đó `dotnet ef database drop --project src/FlashOffer.API.Infrastructure --startup-project src/FlashOffer.API.WebApi` |

---

## 9. 🧪 TESTING - QUY TẮC

### Unit Test
- Test một đơn vị code nhỏ trong isolation
- Mock tất cả dependencies
- Tốc độ nhanh (ms)
- Test: Validator, Handler/Service, DTO mapping

**Lưu ý với Moq:** Methods có optional parameters (CancellationToken) phải truyền đủ số lượng tham số với `It.IsAny<T>()`

### Validator Test
- Khởi tạo validator trực tiếp, không dùng Service/Mock
- Mock `IStringLocalizer<SharedResource>` khi validator inject localizer
- Setup **tất cả resource keys** mà validator dùng

### Integration Test
- Dùng database thật (InMemory/TestContainer)
- Gọi API endpoint thật
- **BaseIntegrationTest Pattern (BẮT BUỘC):**

```csharp
services.RemoveAll(typeof(ApplicationDbContext));
services.RemoveAll(typeof(IApplicationDbContext));
services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
var dbName = $"TestDb_{Guid.NewGuid()}";
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase(dbName));
services.AddScoped<IApplicationDbContext>(sp => 
    sp.GetRequiredService<ApplicationDbContext>());
```

### Quy tắc cho dự án FlashOffer

| Loại test | Khi nào viết | Thư mục | Cần Base class? |
|-----------|--------------|---------|-----------------|
| Unit Test | Mỗi Validator, Handler, Service | `tests/FlashOffer.API.UnitTests/` | ❌ Không |
| Integration Test | Mỗi Controller (1 file chính) | `tests/FlashOffer.API.IntegrationTests/` | ✅ Cần `BaseIntegrationTest` |

---

## 10. 📁 CẤU TRÚC THƯ MỤC & NAMESPACE MAPPING

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
| `IUserService` | `FlashOffer.API.Application.Common.Interfaces` |
| `ApiControllerBase` | `FlashOffer.API.WebApi` |
| `SharedResource` | `FlashOffer.API.Application.Resources` |

---

## 11. 🚨 LƯU Ý QUAN TRỌNG

- `Repository.AddAsync` cần `SaveChangesAsync()` sau đó
- Logic nghiệp vụ đặt trong Service/Handler, không trong Controller
- **BẮT BUỘC** cấu hình `SuppressModelStateInvalidFilter = true`
- **Mọi message client** đều qua `IStringLocalizer`
- **Resource keys:** Tuân theo quy tắc prefix tại mục 5.3
- **Enum:** ưu tiên dùng thay vì string, cấu hình `HasConversion<int>()`
- **Phone validation:** rule 5.2
- **Excel:** format date `yyyy-MM-dd HH:mm:ss`, số `#,##0`, căn trái tất cả
- **User handling:** Tuân theo quy tắc 5.13 khi tạo/lấy dữ liệu
- **Soft Delete:** Luôn dùng xóa mềm, không xóa cứng dữ liệu. Sử dụng `Restore()` khi cần khôi phục.
- **Global Query Filter:** Đã tự động filter `IsDeleted = false`, không cần thêm điều kiện trong repository methods.
- **Expression Extensions:** Dùng `ExpressionExtensions.And()` để gộp predicate, không tự viết `CombinePredicates` trong Service.
- **Queryable Extensions:** Dùng `IncludeMultiple()` hoặc `IncludeThen()` thay vì tạo method cứng cho từng Entity.

---

## 12. 📋 TỔNG HỢP QUY TẮC CHÍNH

| STT | Quy tắc | Áp dụng |
|-----|---------|---------|
| 1 | **Behavior pipeline** | Tất cả MediatR Command/Query |
| 2 | **ITransactionalRequest interface** | Command cần transaction |
| 3 | **Exception + IStringLocalizer** | Mọi lỗi business |
| 4 | **Resource key prefix** | Tất cả message |
| 5 | **Auto-Save Repository** | Service không gọi SaveChanges |
| 6 | **ApiControllerBase** | Tất cả Controller |
| 7 | **ApiVersion** | Tất cả API endpoint |
| 8 | **Ok/OkPaged** | Response chuẩn |
| 9 | **IMapFrom** | Tất cả DTO và Command |
| 10 | **IStringLocalizer trong Validator** | Validation message |
| 11 | **ExpressionExtensions.And()** | Gộp predicate trong Service |
| 12 | **IncludeMultiple()/IncludeThen()** | Include navigation trong Repository |
| 13 | **ICurrentUserService** | Xử lý User trong API |
| 14 | **Soft Delete** | Xóa mềm tất cả Entity |
| 15 | **BaseIntegrationTest** | Integration Test |