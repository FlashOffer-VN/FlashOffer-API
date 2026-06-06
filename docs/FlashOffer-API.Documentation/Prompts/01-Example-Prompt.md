# Prompt template cho AI

## Mô tả
Đây là template prompt dùng cho AI khi thêm feature CRUD mới vào dự án `dotnet-api-base`.

## Nguyên tắc chính
- Luôn ưu tiên `docs/FlashOffer-API.Documentation/` nếu nội dung local khác với kiến thức chung.
- Dự án sử dụng Clean Architecture, AutoMapper, FluentValidation, soft delete và chuẩn `ApiResponse<T>`.
- Chỉ tạo/sửa file cần thiết cho feature mới.
- Tránh thay đổi các file quan trọng trừ khi thực sự cần thiết.
- Build solution và kiểm tra compile sau khi hoàn thành.

## Format prompt
Sử dụng cấu trúc: 
- `Context:` mô tả ngắn về repo
- `Task:` mô tả feature cần tạo
- `Constraints:` các giới hạn và quy tắc
- `Done when:` chỉ ra tiêu chí hoàn thành

## Mẫu prompt
Add Brand: Name*, Description (max 500)

## Checklist AI cần thực hiện
1. Đọc `docs/FlashOffer-API.Documentation/` để nắm convention và rule.
2. Xác định công việc dựa trên template thêm feature mới của repo.
3. Tạo các file sau:
   - `src/FlashOffer-API.Domain/Entities/{EntityName}.cs`
   - `src/FlashOffer-API.Infrastructure/Data/Configurations/{EntityName}Configuration.cs`
   - `src/FlashOffer-API.Application/DTOs/{EntityName}Dto.cs`, `Create{EntityName}Dto.cs`, `Update{EntityName}Dto.cs`
   - `src/FlashOffer-API.Application/Validators/{EntityName}Validators.cs`
   - `src/FlashOffer-API.WebApi/Controllers/v1/{EntityName}Controller.cs`
4. Đảm bảo DTO sử dụng `IMapFrom<T>` để AutoMapper tự nhận mapping.
5. Thêm `DbSet<{EntityName}>` vào `src/FlashOffer-API.Infrastructure/Data/ApplicationDbContext.cs`.
6. Build solution để xác nhận không có lỗi.
7. Nếu có thay đổi liên quan tới cấu trúc, template hoặc khởi tạo repo, chạy `scripts/validate-docs-sync.ps1` để kiểm tra đồng bộ docs.
8. Tổng hợp các file đã tạo/sửa và trả lời ngắn gọn.

## Ví dụ prompt cụ thể
> Context: `dotnet-api-base` là dự án Clean Architecture có docs local tại `docs/FlashOffer-API.Documentation/`.
> Task: Thêm feature `Brand` với các trường:
> - `Name` (required, max 200)
> - `Description` (optional, max 500)
> Constraints: Không sửa file quan trọng trừ khi cần, ưu tiên local docs, phải tạo entity/config/DTO/validator/controller/DbSet, build để kiểm tra.
> Done when: code compile thành công, endpoint CRUD tồn tại, validation đúng.

## Lưu ý cải thiện
- Nếu prompt chỉ có nội dung ngắn như `Add Brand: Name*, Description (max 500)`, AI vẫn phải hiểu task là tạo feature CRUD đầy đủ theo convention của repo.
- Nếu local docs có rule cụ thể, ưu tiên áp dụng chúng hơn kiến thức chung.
- Nếu thấy model `Product` hoặc `SampleController`, hãy dùng nó làm mẫu định dạng.

## Template repo bootstrap
Nếu cần dùng repo này như starter template, AI nên hỗ trợ các bước bootstrap sau:
- Giữ nguyên cấu trúc `src/Domain`, `src/Application`, `src/Infrastructure`, `src/WebApi`.
- Chạy `scripts/rename-project.ps1 -NewProjectName <TênProject>` để đổi tên project và namespace.
- Chạy `scripts/init-template.ps1` để tạo file `.env`, restore packages và build solution.
- Nếu cần Docker env, dùng `scripts/init-template.ps1 -UseDockerEnv`.
- Nếu cần kiểm tra toàn bộ, dùng `scripts/init-template.ps1 -RunTests`.

### Ví dụ prompt bootstrap
> Context: `dotnet-api-base` là starter repo template với scripts bootstrap.
> Task: Đổi tên project thành `MyApiApp`, tạo `.env` từ `.env.example`, restore và build.
> Constraints: chỉ dùng script có sẵn, không sửa logic core của repo.
> Done when: repo được đổi tên, build thành công.

