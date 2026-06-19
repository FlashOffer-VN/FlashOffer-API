# Hướng dẫn Validation - Rule tập trung

## File chính

| File                 | Đường dẫn                                                     | Vai trò                                      |
|----------------------|---------------------------------------------------------------|----------------------------------------------|
| ValidationFilter.cs  | src/FlashOffer.API.WebApi/Filters/ValidationFilter.cs          | Tự động validate mọi request                 |
| Các Validator        | src/FlashOffer.API.Application/Validators/*.cs                 | Rule validation cho từng DTO                 |

## Cách hoạt động

1. ValidationFilter được đăng ký toàn cục trong Program.cs
2. Mọi request có [FromBody] đều được validate tự động
3. Nếu validation fail, tự động trả về 400 BadRequest với danh sách lỗi

## Thêm validation rule mới

Chỉ cần tạo 1 file validator mới - không cần sửa controller hay filter.

Ví dụ: Validator cho CreateProductDto

public class CreateProductValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên không được để trống")
            .MaximumLength(200).WithMessage("Tên tối đa 200 ký tự");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Giá phải lớn hơn 0");
    }
}

## Nếu muốn đổi format lỗi validation

Chỉ cần sửa 1 file: ValidationFilter.cs

## Các rule thường dùng

| Rule          | Ví dụ                                                       | Mô tả                        |
|---------------|-------------------------------------------------------------|------------------------------|
| NotEmpty      | RuleFor(x => x.Name).NotEmpty()                             | Không được null hoặc empty   |
| MaximumLength | RuleFor(x => x.Name).MaximumLength(100)                     | Giới hạn độ dài              |
| GreaterThan   | RuleFor(x => x.Price).GreaterThan(0)                        | Lớn hơn giá trị              |
| EmailAddress  | RuleFor(x => x.Email).EmailAddress()                        | Đúng định dạng email         |
| Matches       | RuleFor(x => x.Phone).Matches("^[0-9]+$")                   | Khớp regex                   |
| Must          | RuleFor(x => x).Must(x => x.StartDate < x.EndDate)          | Custom logic                 |

## Lưu ý

- Validator được tự động scan nhờ AddValidatorsFromAssembly
- KHÔNG cần gọi validator thủ công trong controller
- KHÔNG cần kiểm tra ModelState.IsValid

### Localization in Validators

- If a validator injects IStringLocalizer<T> (for localized messages), ensure services.AddLocalization() is called before AddControllers().AddFluentValidation(...) so the localizer is resolvable when validators are registered.
- Example validator constructor:

```csharp
public class CreateProductValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductValidator(IStringLocalizer<SharedResource> L)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(L["NameRequired"])
            .MaximumLength(200).WithMessage(L["NameMaxLength"]);
    }
}
```
