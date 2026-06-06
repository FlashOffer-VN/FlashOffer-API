# HÆ°á»›ng dáº«n PhÃ¢n trang (Pagination)

Dá»± Ã¡n sá»­ dá»¥ng `PagedList<T>` Ä‘á»ƒ chuáº©n hÃ³a viá»‡c tráº£ vá» dá»¯ liá»‡u danh sÃ¡ch cÃ³ phÃ¢n trang.

## 1. Cáº¥u trÃºc PagedList<T>
Náº±m táº¡i: `src/FlashOffer.API.Application/Common/Models/PagedList.cs`
Bao gá»“m:
- `Items`: Danh sÃ¡ch dá»¯ liá»‡u trang hiá»‡n táº¡i.
- `PageNumber`: Sá»‘ trang hiá»‡n táº¡i.
- `TotalPages`: Tá»•ng sá»‘ trang.
- `TotalCount`: Tá»•ng sá»‘ báº£n ghi.

## 2. CÃ¡ch dÃ¹ng trong Repository
Sá»­ dá»¥ng phÆ°Æ¡ng thá»©c `GetPagedAsync` cÃ³ sáºµn trong `IRepository<T>`:

```csharp
var products = await _repository.GetPagedAsync(pageNumber, pageSize, x => x.Active);
```

## 3. VÃ­ dá»¥ trong Controller

```csharp
[HttpGet("paged")]
public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
{
    var pagedProducts = await _repository.GetPagedAsync(pageNumber, pageSize);

    // Sá»­ dá»¥ng Extension method Ä‘á»ƒ map tá»± Ä‘á»™ng cáº£ PagedList
    var result = _mapper.MapPagedList<Product, ProductDto>(pagedProducts);

    return Ok(result);
}
```