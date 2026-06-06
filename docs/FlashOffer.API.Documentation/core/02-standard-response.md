# Chuẩn Response API - Sửa 1 chỗ, ảnh hưởng tất cả API

## File chính

| File                 | Đường dẫn                                                 | Vai trò                              |
|----------------------|-----------------------------------------------------------|--------------------------------------|
| ApiResponse.cs       | src/FlashOffer.API.WebApi/Responses/ApiResponse.cs         | Định nghĩa format response           |
| ApiControllerBase.cs | src/FlashOffer.API.WebApi/Controllers/ApiControllerBase.cs | Base controller với các method chuẩn |

## Format response hiện tại

### Thành công (Success)

{
  "success": true,
  "message": "Success",
  "data": { ... },
  "errors": null,
  "timestamp": "2024-01-01T00:00:00Z"
}

### Thất bại (Error)

{
  "success": false,
  "message": "Validation failed",
  "data": null,
  "errors": ["Lỗi 1", "Lỗi 2"],
  "timestamp": "2024-01-01T00:00:00Z"
}

## Cách dùng trong Controller

Tất cả Controller phải kế thừa ApiControllerBase

public class SampleController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _repository.GetAllAsync();
        return Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDto dto)
    {
        return Ok(result);
    }
}

## Các method có sẵn trong ApiControllerBase

| Method                                    | HTTP Status | Khi nào dùng                          |
|-------------------------------------------|-------------|---------------------------------------|
| Ok<T>(T data, string message)             | 200         | Trả về data thành công                |
| BadRequest(string message, List<string> errors) | 400   | Lỗi do client                         |
| NotFound(string message)                  | 404         | Không tìm thấy resource               |
| Created<T>(string location, T data, string message) | 201 | Tạo mới thành công                  |
| NoContent(string message)                 | 200         | Xóa hoặc cập nhật thành công          |

## Nếu muốn đổi format response

Chỉ cần sửa 1 file duy nhất: ApiResponse.cs

Ví dụ: Thêm trường requestId vào response

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
    public DateTime Timestamp { get; set; }
    public string RequestId { get; set; }

    public ApiResponse()
    {
        Timestamp = DateTime.UtcNow;
        RequestId = Guid.NewGuid().ToString();
    }
}

## Lưu ý

- KHÔNG dùng return Ok(object) trực tiếp từ Controller base
- LUÔN dùng return Ok(data) từ ApiControllerBase
- KHÔNG tự tạo ApiResponse<T> thủ công trong Controller