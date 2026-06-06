# Dependency Injection - Đăng ký service tập trung

## File chính

| File                                    | Đường dẫn                                                       | Vai trò                                                |
|-----------------------------------------|-----------------------------------------------------------------|--------------------------------------------------------|
| DependencyInjection.cs (Application)    | src/FlashOffer-API.Application/DependencyInjection.cs            | Đăng ký services của Application layer                 |
| DependencyInjection.cs (Infrastructure) | src/FlashOffer-API.Infrastructure/DependencyInjection.cs         |   Đăng ký services của Infrastructure layer            |
| DependencyInjection.cs (WebApi)         | src/FlashOffer-API.WebApi/DependencyInjection.cs                 | Tổng hợp và đăng ký API-specific services              |

## Cách hoạt động

Program.cs chỉ cần gọi 1 dòng duy nhất:

builder.Services.AddWebApiServices(builder.Configuration);

## Application Layer - Các service đã đăng ký

| Service              | Mô tả                                      |
|----------------------|--------------------------------------------|
| AutoMapper           | Scan toàn bộ assembly                      |
| FluentValidation     | Scan toàn bộ validator                     |

## Infrastructure Layer - Các service đã đăng ký

| Service                 | Mô tả                                      |
|-------------------------|--------------------------------------------|
| ApplicationDbContext    | DbContext chính                            |
| IApplicationDbContext   | Interface cho DbContext                    |
| GenericRepository       | Cho tất cả entity                          |
| JwtService              | JWT authentication service                 |

## WebApi Layer - Các service đã đăng ký

| Service                 | Mô tả                                      |
|-------------------------|--------------------------------------------|
| JWT Settings            | Cấu hình JWT                               |
| Authentication          | JWT Bearer                                 |
| API Versioning          | Versioning support                         |
| Controllers             | Với ValidationFilter                       |
| Swagger                 | API documentation                          |

## Thêm service mới

Ví dụ: Thêm ICategoryService

// Bước 1: Tạo interface trong Application layer
public interface ICategoryService
{
    Task<bool> IsNameExistsAsync(string name);
}

// Bước 2: Implement trong Infrastructure layer
public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _repository;
    public CategoryService(IRepository<Category> repository)
    {
        _repository = repository;
    }
    public async Task<bool> IsNameExistsAsync(string name)
    {
        return await _repository.AnyAsync(x => x.Name == name);
    }
}

// Bước 3: Đăng ký trong Infrastructure/DependencyInjection.cs
services.AddScoped<ICategoryService, CategoryService>();

## Lưu ý

- Đăng ký đúng lifetime: AddSingleton, AddScoped, AddTransient
- Repository đã được đăng ký generic, không cần đăng ký từng entity
- KHÔNG đăng ký service trong Program.cs trực tiếp