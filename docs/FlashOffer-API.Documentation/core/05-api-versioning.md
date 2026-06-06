# API Versioning - Hỗ trợ nhiều phiên bản API

## File chính

| File                    | Đường dẫn                                                                   | Vai trò                                          |
|-------------------------|-----------------------------------------------------------------------------|--------------------------------------------------|
| ApiVersioningConfig.cs  | src/FlashOffer-API.WebApi/Configurations/ApiVersioningConfig.cs              |  Cấu hình versioning cho toàn bộ API             |

## Cách hoạt động

1. Đăng ký trong DependencyInjection.cs thông qua AddApiVersioningConfig()
2. Hỗ trợ URL segment versioning: /api/v1/controller
3. Mặc định version 1.0 nếu không chỉ định

## Cấu hình hiện tại

| Cấu hình                                   | Giá trị                                          | Mô tả                               |
|--------------------------------------------|--------------------------------------------------|-------------------------------------|
| DefaultApiVersion                          | new ApiVersion(1, 0)                             | Version mặc định                    |
| AssumeDefaultVersionWhenUnspecified        | true                                             | Tự động gán version mặc định        |
| ReportApiVersions                          | true                                             | Trả về version trong response header|
| ApiVersionReader                           | new UrlSegmentApiVersionReader()                 | Lấy version từ URL                  |

## Cách dùng trong Controller

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class SampleController : ApiControllerBase
{
    // API sẽ có dạng: /api/v1/sample
}

## Thêm version mới

1. Tạo thư mục mới: Controllers/v2/
2. Tạo controller mới với [ApiVersion("2.0")]
3. Có thể copy logic từ version cũ hoặc viết mới

## Lưu ý

- Backward compatibility: Version cũ vẫn hoạt động
- Không xóa version cũ khi thêm version mới
- Deprecated version: Đánh dấu [ApiVersion("1.0", Deprecated = true)]