# Feature: Brand Management

## Ngày tạo
2024-05-24

## Mô tả
API quản lý Brand (Thương hiệu) với các chức năng CRUD cơ bản và phân trang.

## Các file đã tạo

### Domain Layer
- `src/FlashOffer-API.Domain/Entities/Brand.cs`

### Infrastructure Layer
- `src/FlashOffer-API.Infrastructure/Data/Configurations/BrandConfiguration.cs`

### Application Layer - DTOs
- `src/FlashOffer-API.Application/DTOs/BrandDto.cs`
- `src/FlashOffer-API.Application/DTOs/CreateBrandDto.cs`
- `src/FlashOffer-API.Application/DTOs/UpdateBrandDto.cs`

### WebApi Layer
- `src/FlashOffer-API.WebApi/Controllers/v1/BrandController.cs`

## Migration
- Migration name: `AddBrandTable`
- Table name: `Brands`

## API Endpoints

| Method | Endpoint | Auth |
|--------|----------|------|
| GET | `/api/v1/brand/paged` | None |
| GET | `/api/v1/brand/{id}` | None |
| POST | `/api/v1/brand` | JWT Required |
| PUT | `/api/v1/brand/{id}` | JWT Required |
| DELETE | `/api/v1/brand/{id}` | JWT Required |