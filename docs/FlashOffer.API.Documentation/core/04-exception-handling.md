# Xử lý lỗi tập trung - Global Exception Middleware

## File chính

| File                          | Đường dẫn                                                             | Vai trò                              |
|-------------------------------|-----------------------------------------------------------------------|--------------------------------------|
| GlobalExceptionMiddleware.cs  | src/FlashOffer.API.WebApi/Middlewares/GlobalExceptionMiddleware.cs     | Bắt mọi exception chưa được xử lý    |

## Cách hoạt động

1. Middleware được đăng ký trong Program.cs
2. Bắt tất cả exception từ các layer bên dưới
3. Log lỗi và trả về response chuẩn

## Format lỗi hiện tại

{
  "success": false,
  "message": "An error occurred while processing your request.",
  "data": null,
  "errors": ["Chi tiết lỗi..."],
  "timestamp": "2024-01-01T00:00:00Z"
}

## Nếu muốn đổi format lỗi

Chỉ cần sửa 1 file: GlobalExceptionMiddleware.cs

Tìm method HandleExceptionAsync trong file này và sửa response object theo ý muốn.

## Nếu muốn xử lý riêng cho từng loại exception

catch (ValidationException ex)
{
    // Xử lý validation exception
    return HandleValidationException(context, ex);
}
catch (DbUpdateException ex)
{
    // Xử lý database exception
    return HandleDbException(context, ex);
}
catch (Exception ex)
{
    // Xử lý exception chung
    return HandleGenericException(context, ex);
}

## Lưu ý

- KHÔNG dùng try-catch lung tung trong controller
- Chỉ try-catch khi cần xử lý đặc biệt
- Để GlobalExceptionMiddleware xử lý phần còn lại