# Tài liệu dự án FlashOffer.API

Tài liệu này giúp **AI và Developer** hiểu cách dự án hoạt động.

## 📁 Cấu trúc tài liệu

| Thư mục | Mô tả |
|---------|-------|
| core/ | Tài liệu cốt lõi, bắt buộc đọc |
| guides/ | Hướng dẫn bổ sung (Docker, CI/CD, troubleshooting) |
| prompts/ | Prompt mẫu cho AI |

## 📚 Core (Đọc trước)

| File | Mô tả |
|------|-------|
| [00-ai-rules.md](core/00-ai-rules.md) | Quy tắc AI |
| [01-architecture.md](core/01-architecture.md) | Kiến trúc Clean Architecture |
| [02-standard-response.md](core/02-standard-response.md) | Chuẩn response API |
| [03-validation-guide.md](core/03-validation-guide.md) | Validation rules |
| [04-exception-handling.md](core/04-exception-handling.md) | Xử lý lỗi tập trung |
| [05-api-versioning.md](core/05-api-versioning.md) | API versioning |
| [06-dependency-injection.md](core/06-dependency-injection.md) | DI registration |
| [07-automapper.md](core/07-automapper.md) | Entity-DTO mapping |
| [08-repository-pattern.md](core/08-repository-pattern.md) | Repository pattern |
| [09-authentication.md](core/09-authentication.md) | JWT Authentication |
| [10-logging.md](core/10-logging.md) | Logging với Serilog |
| [11-how-to-add-new-api.md](core/11-how-to-add-new-api.md) | Thêm API mới |
| [12-code-conventions.md](core/12-code-conventions.md) | Quy tắc code |
| [13-rule-write-docs.md](core/13-rule-write-docs.md) | Quy tắc viết docs |

## 📘 Guides (Tham khảo)

| File | Mô tả |
|------|-------|
| [14-project-bootstrap.md](guides/14-project-bootstrap.md) | Khởi tạo dự án mới và sử dụng repo như starter template |
| [ci-cd.md](guides/ci-cd.md) | CI/CD GitHub Actions |
| [docker-ci-cd-guide.md](guides/docker-ci-cd-guide.md) | Docker và CI/CD |
| [docker-compose.md](guides/docker-compose.md) | Docker Compose |
| [env-vars.md](guides/env-vars.md) | Biến môi trường |
| [troubleshooting.md](guides/troubleshooting.md) | Xử lý lỗi |

## 🤖 Prompts

Thư mục prompts/ chứa các prompt mẫu để gửi cho AI.

## � Quy tắc đồng bộ docs

- Mọi thay đổi cấu trúc, kiến trúc, template hoặc quy trình bootstrap phải được phản ánh trong `docs/FlashOffer.API.Documentation/` hoặc `README.md`.
- Nếu sửa `scripts/rename-project.ps1`, `scripts/init-template.ps1`, `README.md`, `Dockerfile`, `docker-compose.yml`, hoặc các file cấu hình môi trường, hãy cập nhật docs tương ứng.
- Chạy `scripts/validate-docs-sync.ps1` mỗi khi thay đổi cấu trúc hoặc template để xác nhận docs đã được đồng bộ.

## �📝 Luồng code khi thêm feature mới

1. Domain Entity
2. DTOs (Create, Update, Response)
3. Validator (FluentValidation)
4. AutoMapper Mapping
5. DbSet (Infrastructure)
6. Controller (kế thừa ApiControllerBase)
7. Migration

## ⚠️ File quan trọng - Không sửa lung tung

- WebApi/Responses/ApiResponse.cs
- WebApi/Controllers/ApiControllerBase.cs
- WebApi/Middlewares/GlobalExceptionMiddleware.cs
- WebApi/Filters/ValidationFilter.cs
- Application/Mappings/MappingProfile.cs

## ✅ File được phép thêm mới

- Domain/Entities/*.cs
- Application/DTOs/*.cs
- Application/Validators/*.cs
- WebApi/Controllers/v1/*.cs
