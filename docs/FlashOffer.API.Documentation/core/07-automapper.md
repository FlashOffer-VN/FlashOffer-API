# AutoMapper - Entity-DTO Mapping

## File chính

| File                 | Đường dẫn                                                             | Vai trò                                              |
|----------------------|-----------------------------------------------------------------------|------------------------------------------------------|
| MappingProfile.cs    | src/FlashOffer.API.Application/Mappings/MappingProfile.cs              | Định nghĩa tất cả mapping trong 1 file duy nhất      |

## Cách hoạt động

1. AutoMapper được đăng ký trong Application/DependencyInjection.cs
2. services.AddAutoMapper(Assembly.GetExecutingAssembly())
3. Tất cả Profile được tự động scan và đăng ký

## Các mapping hiện tại

| Source              | Destination          | Ghi chú                                                           |
|---------------------|----------------------|-------------------------------------------------------------------|
| Product             | ProductDto           | Entity -> DTO                                                     |
| CreateProductDto    | Product              | DTO -> Entity                                                     |
| UpdateProductDto    | Product              | Update DTO -> Entity (ignore Id, CreatedAt, UpdatedAt, IsDeleted) |

## Thêm mapping mới cho entity mới

Thêm vào cuối file MappingProfile.cs:

CreateMap<Category, CategoryDto>();
CreateMap<CreateCategoryDto, Category>();
CreateMap<UpdateCategoryDto, Category>()
    .ForMember(dest => dest.Id, opt => opt.Ignore())
    .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
    .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
    .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

## Cách dùng trong Controller

| Cách dùng                                   | Code                                                        |
|---------------------------------------------|-------------------------------------------------------------|
| Entity -> DTO                               | var dtos = _mapper.Map<IEnumerable<CategoryDto>>(entities); |
| DTO -> Entity                               | var entity = _mapper.Map<Category>(createDto);              |
| Update existing from DTO                    | _mapper.Map(updateDto, existingEntity);                     |

## Các mapping nâng cao

| Nhu cầu                     | Cú pháp                                                                                  |
|-----------------------------|------------------------------------------------------------------------------------------|
| Khác tên property           | .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))                    |
| Bỏ qua property             | .ForMember(dest => dest.Id, opt => opt.Ignore())                                         |
| Custom value                | .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.Name} - {src.Code}")) |
| Nested object               | .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems))                |

## Lưu ý

- KHÔNG dùng AutoMapper trong Domain layer
- Profile được tự động scan, không cần đăng ký thủ công
- Inject IMapper vào constructor của Controller