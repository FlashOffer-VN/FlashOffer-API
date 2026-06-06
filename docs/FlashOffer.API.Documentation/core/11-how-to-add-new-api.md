@"
# Hướng dẫn thêm API mới - Step by Step

## Luồng tổng quát (Làm theo thứ tự)

| Bước | Nội dung | Loại file |
|------|----------|-----------|
| 1 | Domain Entity | Tạo mới |
| 2 | Entity Configuration (Fluent API) | Tạo mới |
| 3 | DTOs (Create, Update, Response) | Tạo mới |
| 4 | Validator (FluentValidation) | Tạo mới |
| 5 | AutoMapper Mapping (MappingProfile.cs) | Sửa |
| 6 | DbSet (ApplicationDbContext.cs) | Sửa |
| 7 | Controller (kế thừa ApiControllerBase) | Tạo mới |
| 8 | Migration | Chạy lệnh |

## Ví dụ: Thêm API quản lý Category

### Bước 1: Domain Entity

Tạo file: src/FlashOffer.API.Domain/Entities/Category.cs

namespace FlashOffer.API.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

### Bước 2: Entity Configuration (QUAN TRỌNG - MỚI)

Tạo file: src/FlashOffer.API.Infrastructure/Data/Configurations/CategoryConfiguration.cs

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FlashOffer.API.Domain.Entities;

namespace FlashOffer.API.Infrastructure.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Description).HasMaxLength(500);
    }
}

### Bước 3: DTOs

Tạo file: src/FlashOffer.API.Application/DTOs/CategoryDtos.cs

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class CreateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdateCategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

### Bước 4: Validator

Tạo file: src/FlashOffer.API.Application/Validators/CategoryValidators.cs

using FluentValidation;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required")
            .MaximumLength(100).WithMessage("Name max 100 chars");
    }
}

### Bước 5: AutoMapper Mapping

Thêm vào cuối file: src/FlashOffer.API.Application/Mappings/MappingProfile.cs

// Không cần thêm thủ công nếu DTO đã kế thừa IMapFrom<T>.
// MappingProfile tự động quét qua ApplyMappingsFromAssembly.

### Bước 6: DbSet

Thêm vào file: src/FlashOffer.API.Infrastructure/Data/ApplicationDbContext.cs

public DbSet<Category> Categories => Set<Category>();

LƯU Ý: KHÔNG cần thêm cấu hình Fluent API vào OnModelCreating nữa.
Tất cả cấu hình đã được tách riêng trong file Configuration.

### Bước 7: Controller

Tạo file: src/FlashOffer.API.WebApi/Controllers/v1/CategoryController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs;
using FlashOffer.API.Domain.Entities;

namespace FlashOffer.API.WebApi.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/category")]
public class CategoryController : CrudControllerBase<Category, CategoryDto, CreateCategoryDto, UpdateCategoryDto>
{
    public CategoryController(IRepository<Category> repository, IMapper mapper) 
        : base(repository, mapper)
    {
    }
}

### Bước 8: Migration

cd src/FlashOffer.API.WebApi
dotnet ef migrations add AddCategoryTable
dotnet ef database update
cd ../..

## Tóm tắt các file cần thêm hoặc sửa

| STT | Loại | File | Hành động |
|-----|------|------|-----------|
| 1 | Thêm mới | Domain/Entities/Category.cs | Tạo mới |
| 2 | Thêm mới | Infrastructure/Data/Configurations/CategoryConfiguration.cs | Tạo mới |
| 3 | Thêm mới | Application/DTOs/CategoryDtos.cs | Tạo mới |
| 4 | Thêm mới | Application/Validators/CategoryValidators.cs | Tạo mới |
| 5 | Sửa | Application/Mappings/MappingProfile.cs | Thêm mapping |
| 6 | Sửa | Infrastructure/Data/ApplicationDbContext.cs | Thêm DbSet |
| 7 | Thêm mới | WebApi/Controllers/v1/CategoryController.cs | Tạo mới |
| 8 | Chạy lệnh | Migration | dotnet ef migrations add |

## Lưu ý quan trọng

| STT | Lưu ý |
|-----|-------|
| 1 | Ưu tiên kế thừa `CrudControllerBase` để có sẵn 5 endpoints CRUD |
| 2 | Luôn dùng IRepository<T> để truy xuất database |
| 3 | Luôn dùng IMapper để map Entity và DTO |
| 4 | GET endpoints thêm [AllowAnonymous] nếu muốn công khai |
| 5 | POST, PUT, DELETE không cần decorate vì controller có [Authorize] |
| 6 | **MỚI:** Tạo file Configuration riêng cho mỗi entity, KHÔNG thêm Fluent API vào DbContext |
"@ | Out-File -FilePath docs/FlashOffer.API.Documentation/core/11-how-to-add-new-api.md -Encoding UTF8