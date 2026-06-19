# Quy tắc code - Coding Conventions

## Đặt tên

| Thành phần             | Quy tắc                  | Ví dụ                                        |
|------------------------|--------------------------|----------------------------------------------|
| Class, Method, Property | PascalCase              | ProductController, GetProductById, FirstName |
| Interface              | I + PascalCase           | IRepository, IProductService                 |
| Private field          | _camelCase               | _repository, _mapper, _logger                |
| Parameter, Variable    | camelCase                | productId, productName, createDto            |
| Constant               | UPPER_SNAKE_CASE         | MAX_RETRY_COUNT, DEFAULT_PAGE_SIZE           |

## File organization

| Quy tắc                         | Mô tả                                                |
|---------------------------------|------------------------------------------------------|
| Mỗi class một file              | Tên file trùng với tên class                         |
| Folder structure theo layer     | Domain/Entities/*.cs, Application/DTOs/*.cs          |

Chi tiết folder structure:

- Domain/Entities/*.cs
- Application/DTOs/*.cs
- Application/Validators/*.cs
- Application/Mappings/MappingProfile.cs
- Infrastructure/Data/*.cs
- Infrastructure/Repositories/*.cs
- WebApi/Controllers/v1/*.cs

## Async pattern

| Quy tắc                                 | Ví dụ                                              |
|-----------------------------------------|----------------------------------------------------|
| Tên method kết thúc bằng Async          | GetByIdAsync, SaveChangesAsync                     |
| Return type: Task<T> hoặc Task          | public async Task<Product> GetByIdAsync(Guid id)   |
| KHÔNG async void (trừ event handler)    | -                                                  |
| LUÔN await async method                 | await _repository.GetByIdAsync(id)                 |

Ví dụ:

public async Task<Product> GetByIdAsync(Guid id)
{
    return await _repository.GetByIdAsync(id);
}

## Error handling

| Layer              | Quy tắc                                                                 |
|--------------------|-------------------------------------------------------------------------|
| Controller         | KHÔNG try-catch trong controller, để GlobalExceptionMiddleware xử lý    |
| Service/Handler    | Chỉ try-catch khi cần xử lý đặc biệt, throw exception để middleware bắt |

Ví dụ:

public async Task<Product> GetByIdAsync(Guid id)
{
    var product = await _repository.GetByIdAsync(id);
    if (product == null)
        throw new NotFoundException($"Product {id} not found");
    return product;
}

## Response format

| Nên dùng                           | Không dùng                          |
|------------------------------------|-------------------------------------|
| return Ok(data)                    | return Ok(object) trực tiếp         |
| return BadRequest(message)         | return new ApiResponse<T>()         |
| return NotFound(message)           | -                                   |
| return NoContent(message)          | -                                   |

## Dependency Injection

| Quy tắc                                 | Ví dụ                                           |
|-----------------------------------------|-------------------------------------------------|
| Chỉ inject interface                    | IRepository<Product>                            |
| KHÔNG inject class cụ thể               | -                                               |
| KHÔNG new object trong constructor      | -                                               |

Ví dụ:

public ProductController(IRepository<Product> repository, IMapper mapper)
{
    _repository = repository;
    _mapper = mapper;
}

### Testing guidelines

- Unit tests should mock external dependencies using Moq and explicitly include CancellationToken parameters in setups when the method signature includes them (e.g., It.IsAny<CancellationToken>()).
- Integration tests should use the provided BaseIntegrationTest which creates a deterministic InMemory database name and seeds data using Factory.Server.Services.CreateScope().
- Prefer Guid for entity Ids; if using numeric Ids, document the choice in the entity's file header.

## Validation

| Quy tắc                                 | Ví dụ                                           |
|-----------------------------------------|-------------------------------------------------|
| LUÔN tạo validator cho DTO              | CreateProductValidator                          |
| KHÔNG validate trong controller         | -                                               |

Ví dụ:

public class CreateProductValidator : AbstractValidator<CreateProductDto>

## Summary comments

| Loại              | Yêu cầu                                         |
|-------------------|-------------------------------------------------|
| Public API        | Cần summary                                     |
| Internal code     | Không bắt buộc                                  |

Ví dụ:

/// <summary>
/// Gets product by id
/// </summary>
/// <param name="id">Product identifier</param>
/// <returns>Product details</returns>