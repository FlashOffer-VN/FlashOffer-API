# Hướng dẫn Phân trang (Pagination)

Dự án sử dụng `PagedList<T>` để chuẩn hóa việc trả về dữ liệu danh sách có phân trang.

## 1. Cấu trúc PagedList<T>
Nằm tại: `src/FlashOffer-API.Application/Common/Models/PagedList.cs`
Bao gồm:
- `Items`: Danh sách dữ liệu trang hiện tại.
- `PageNumber`: Số trang hiện tại.
- `TotalPages`: Tổng số trang.
- `TotalCount`: Tổng số bản ghi.

## 2. Cách dùng trong Repository
Sử dụng phương thức `GetPagedAsync` có sẵn trong `IRepository<T>`:

```csharp
var products = await _repository.GetPagedAsync(pageNumber, pageSize, x => x.Active);
```

## 3. Ví dụ trong Controller

```csharp
[HttpGet("paged")]
public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
{
    var pagedProducts = await _repository.GetPagedAsync(pageNumber, pageSize);

    // Sử dụng Extension method để map tự động cả PagedList
    var result = _mapper.MapPagedList<Product, ProductDto>(pagedProducts);

    return Ok(result);
}
```