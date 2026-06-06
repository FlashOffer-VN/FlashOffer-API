# JWT Authentication - Xác thực người dùng

## File chính

| File                 | Đường dẫn                                                                           | Vai trò                                  |
|----------------------|-------------------------------------------------------------------------------------|------------------------------------------|
| JwtSettings.cs       | src/FlashOffer.API.Infrastructure/Configurations/JwtSettings.cs                      | Cấu hình JWT                             |
| IJwtService.cs       | src/FlashOffer.API.Application/Common/Interfaces/IJwtService.cs                      | Interface cho JWT service                |
| JwtService.cs        | src/FlashOffer.API.Infrastructure/Services/JwtService.cs                             | Implementation JWT service               |
| AuthController.cs    | src/FlashOffer.API.WebApi/Controllers/v1/AuthController.cs                           | API đăng nhập                            |

## Cấu hình trong appsettings.json

{
  "JwtSettings": {
    "Secret": "YourSuperSecretKeyHereAtLeast32CharactersLong!",
    "Issuer": "FlashOffer.API",
    "Audience": "FlashOffer.APIClient",
    "ExpiryMinutes": 60
  }
}

## IJwtService interface

public interface IJwtService
{
    string GenerateToken(string userId, string username, IEnumerable<string> roles);
    ClaimsPrincipal? ValidateToken(string token);
}

## Cách lấy token

POST /api/v1/auth/login

Body:
{
  "username": "admin",
  "password": "password"
}

Response:
{
  "success": true,
  "message": "Success",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIs...",
    "expiresAt": "2024-01-01T00:00:00Z",
    "username": "admin"
  }
}

## Cách dùng token trong request

Header: Authorization: Bearer YOUR_TOKEN_HERE

## Bảo vệ API endpoints

| Mục đích                               | Code                                                    |
|----------------------------------------|---------------------------------------------------------|
| Toàn bộ controller cần xác thực        | [Authorize]                                             |
| Cho phép không cần xác thực            | [AllowAnonymous]                                        |
| Yêu cầu role cụ thể                    | [Authorize(Roles = "Admin")]                            |

## Tích hợp với database (TODO)

Hiện tại đang dùng hardcoded username/password:
- username: admin
- password: password

Cần thay thế bằng kiểm tra từ database:

1. Thêm User entity vào Domain
2. Thêm UserService để kiểm tra password hash
3. Sửa AuthController.Login để gọi UserService

## Lưu ý

- Secret phải dài ít nhất 32 ký tự
- KHÔNG commit secret lên GitHub (dùng User Secrets hoặc Environment Variables)
- Token có thời gian hết hạn (ExpiryMinutes)