# Repository Pattern - Truy xuất dữ liệu

## File chính

| File                 | Đường dẫn                                                                           | Vai trò                                      |
|----------------------|-------------------------------------------------------------------------------------|----------------------------------------------|
| IRepository.cs       | src/FlashOffer-API.Application/Common/Interfaces/IRepository.cs                      | Interface generic cho repository             |
| GenericRepository.cs | src/FlashOffer-API.Infrastructure/Repositories/GenericRepository.cs                  | Implementation generic cho repository        |

## Cấu trúc interface

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<PagedList<T>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Delete(T entity);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

## Các method có sẵn

| Method           | Mô tả                                      | Ví dụ                                                      |
|------------------|--------------------------------------------|------------------------------------------------------------|
| GetByIdAsync     | Lấy entity theo Id                         | await _repository.GetByIdAsync(id)                         |
| GetFirstAsync    | Lấy entity đầu tiên thỏa điều kiện         | await _repository.GetFirstAsync(x => x.Name == name)       |
| GetAllAsync      | Lấy tất cả entity                          | await _repository.GetAllAsync()                            |
| FindAsync        | Lấy danh sách thỏa điều kiện               | await _repository.FindAsync(x => x.Category == cat)        |
| GetPagedAsync    | Lấy danh sách phân trang                   | await _repository.GetPagedAsync(pageNumber, pageSize)      |
| AddAsync         | Thêm entity mới                            | await _repository.AddAsync(entity)                         |
| Update           | Cập nhật entity                            | _repository.Update(entity)                                 |
| Delete           | Xóa entity                                 | _repository.Delete(entity)                                 |
| AnyAsync         | Kiểm tra tồn tại                           | await _repository.AnyAsync(x => x.Name == name)            |
| SaveChangesAsync | Lưu thay đổi vào database                  | await _repository.SaveChangesAsync()                       |

## Cách dùng trong Controller

private readonly IRepository<Product> _repository;

public ProductController(IRepository<Product> repository)
{
    _repository = repository;
}

[HttpGet]
public async Task<IActionResult> GetAll()
{
    var products = await _repository.GetAllAsync();
    return Ok(products);
}

[HttpGet("{id}")]
public async Task<IActionResult> GetById(Guid id)
{
    var product = await _repository.GetByIdAsync(id);
    if (product == null) return NotFound();
    return Ok(product);
}

[HttpPost]
public async Task<IActionResult> Create(Product product)
{
    await _repository.AddAsync(product);
    await _repository.SaveChangesAsync();
    return Ok(product);
}

## Phân trang (Pagination)
Sử dụng `PagedList<T>` để trả về dữ liệu phân trang:
```csharp
var result = await _repository.GetPagedAsync(pageNumber, pageSize, x => x.Price > 100);
```

## Soft delete (IsDeleted)
Hệ thống tự động lọc các bản ghi đã xóa thông qua Global Query Filter:
`entityType.SetQueryFilter(lambda);` (trong `ApplicationDbContext`)

- Khi gọi `_repository.Delete(entity)`, DbContext sẽ tự động chuyển trạng thái thành `Modified` và set `IsDeleted = true`.

## Lưu ý

- Repository đã được đăng ký generic, KHÔNG cần tạo repository riêng cho từng entity
- Luôn gọi SaveChangesAsync sau Add, Update, Delete
- KHÔNG dùng repository trong Domain layer