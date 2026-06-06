# Feature: Brand Management

## NgÃ y táº¡o
2024-05-24

## MÃ´ táº£
API quáº£n lÃ½ Brand (ThÆ°Æ¡ng hiá»‡u) vá»›i cÃ¡c chá»©c nÄƒng CRUD cÆ¡ báº£n vÃ  phÃ¢n trang.

## CÃ¡c file Ä‘Ã£ táº¡o

### Domain Layer
- `src/FlashOffer.API.Domain/Entities/Brand.cs`

### Infrastructure Layer
- `src/FlashOffer.API.Infrastructure/Data/Configurations/BrandConfiguration.cs`

### Application Layer - DTOs
- `src/FlashOffer.API.Application/DTOs/BrandDto.cs`
- `src/FlashOffer.API.Application/DTOs/CreateBrandDto.cs`
- `src/FlashOffer.API.Application/DTOs/UpdateBrandDto.cs`

### WebApi Layer
- `src/FlashOffer.API.WebApi/Controllers/v1/BrandController.cs`

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