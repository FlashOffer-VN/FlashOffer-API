# AI Rules - Quy táº¯c lÃ m viá»‡c vá»›i dá»± Ã¡n FlashOffer.API

## Má»¥c Ä‘Ã­ch

File nÃ y Ä‘á»‹nh nghÄ©a cÃ¡ch AI pháº£i lÃ m viá»‡c khi thÃªm feature má»›i vÃ o dá»± Ã¡n, Ä‘áº£m báº£o AI cÃ³ thá»ƒ tá»± Ä‘á»™ng theo dÃµi vÃ  bÃ¡o cÃ¡o thay Ä‘á»•i.

---

## RULE 1: Template thÃªm feature má»›i

Khi Ä‘Æ°á»£c yÃªu cáº§u thÃªm má»™t feature má»›i (vÃ­ dá»¥: Category, Order, User), AI pháº£i tuÃ¢n theo template sau:

### File cáº§n Táº O Má»šI (7 file)

| STT | File                                                                 | MÃ´ táº£          |
|-----|----------------------------------------------------------------------|----------------|
| 1   | `src/FlashOffer.API.Domain/Entities/{EntityName}.cs`                  | Entity class   |
| 2   | `src/FlashOffer.API.Infrastructure/Data/Configurations/{EntityName}Configuration.cs` | Entity Configuration |
| 3   | `src/FlashOffer.API.Application/DTOs/{EntityName}Dto.cs`              | Response DTO   |
| 4   | `src/FlashOffer.API.Application/DTOs/Create{EntityName}Dto.cs`        | Create DTO     |
| 5   | `src/FlashOffer.API.Application/DTOs/Update{EntityName}Dto.cs`        | Update DTO     |
| 6   | `src/FlashOffer.API.Application/Validators/{EntityName}Validators.cs` | Validator      |
| 7   | `src/FlashOffer.API.WebApi/Controllers/v1/{EntityName}Controller.cs`  | API Controller (káº¿ thá»«a `CrudControllerBase`) |

### File cáº§n Sá»¬A Äá»”I (2 file)

| STT | File                                                            | Thay Ä‘á»•i            |
|-----|-----------------------------------------------------------------|---------------------|
| 1   | `src/FlashOffer.API.Application/Mappings/MappingProfile.cs`      | ThÃªm 3 dÃ²ng mapping |
| 2   | `src/FlashOffer.API.Infrastructure/Data/ApplicationDbContext.cs` | ThÃªm 1 dÃ²ng DbSet   |

### Lá»‡nh cáº§n CHáº Y (2 lá»‡nh)

| STT | Lá»‡nh                                            | MÃ´ táº£             |
|-----|-------------------------------------------------|-------------------|
| 1   | `dotnet ef migrations add Add{EntityName}Table` | Táº¡o migration     |
| 2   | `dotnet ef database update`                     | Cáº­p nháº­t database |

---

## RULE 1.1: PhÃ¢n trang vÃ  Mapping
- Má»i API láº¥y danh sÃ¡ch pháº£i há»— trá»£ phÃ¢n trang sá»­ dá»¥ng `PagedList<T>`.
- Sá»­ dá»¥ng extension `MapPagedList` Ä‘á»ƒ chuyá»ƒn Ä‘á»•i PagedList giá»¯a Entity vÃ  DTO.
- CÃ¡c DTOs pháº£i káº¿ thá»«a `IMapFrom<T>` Ä‘á»ƒ tá»± Ä‘á»™ng mapping.

## RULE 1.2: Repository Methods
- CÃ¡c phÆ°Æ¡ng thá»©c `Update` vÃ  `Delete` trong Repository lÃ  Ä‘á»“ng bá»™ (khÃ´ng dÃ¹ng Async).
- LuÃ´n gá»i `SaveChangesAsync()` Ä‘á»ƒ thá»±c thi thay Ä‘á»•i xuá»‘ng Database.

## RULE 2: BÃ¡o cÃ¡o thay Ä‘á»•i sau khi thÃªm feature

Sau khi thÃªm feature má»›i, AI pháº£i tráº£ vá» bÃ¡o cÃ¡o theo format sau:

## ðŸ“‹ BÃ¡o cÃ¡o thay Ä‘á»•i - Feature: [TÃªn Entity]

### ðŸ“ File Ä‘Æ°á»£c táº¡o má»›i (Create)

| STT | File                                               | MÃ´ táº£               |
|-----|----------------------------------------------------|---------------------|
| 1   | `Domain/Entities/{EntityName}.cs`                  | Entity {EntityName} |
| 2   | `Infrastructure/Data/Configurations/{EntityName}Configuration.cs` | Entity Configuration |
| 3   | `Application/DTOs/{EntityName}Dto.cs`              | Response DTO        |
| 4   | `Application/DTOs/Create{EntityName}Dto.cs`        | Create DTO          |
| 5   | `Application/DTOs/Update{EntityName}Dto.cs`        | Update DTO          |
| 6   | `Application/Validators/{EntityName}Validators.cs` | Validator           |
| 7   | `WebApi/Controllers/v1/{EntityName}Controller.cs`  | API Controller      |

### ðŸ“ File Ä‘Æ°á»£c sá»­a Ä‘á»•i (Modify)

| STT | File                                          | Thay Ä‘á»•i                 |
|-----|-----------------------------------------------|--------------------------|
| 1   | `Application/Mappings/MappingProfile.cs`      | ThÃªm 3 dÃ²ng mapping      |
| 2   | `Infrastructure/Data/ApplicationDbContext.cs` | ThÃªm DbSet<{EntityName}> |

### ðŸ—„ï¸ Migration

| Lá»‡nh                                            | Tráº¡ng thÃ¡i |
|-------------------------------------------------|------------|
| `dotnet ef migrations add Add{EntityName}Table` | âœ… / âŒ   |
| `dotnet ef database update`                     | âœ… / âŒ   |

### âœ… Checklist xÃ¡c nháº­n

- [ ] Entity Ä‘Ã£ káº¿ thá»«a BaseEntity
- [ ] DTOs Ä‘Ã£ Ä‘á»§ 3 loáº¡i
- [ ] Validator Ä‘Ã£ cÃ³ rule cÆ¡ báº£n
- [ ] AutoMapper Ä‘Ã£ thÃªm mapping
- [ ] DbSet Ä‘Ã£ thÃªm
- [ ] Controller Ä‘Ã£ káº¿ thá»«a ApiControllerBase
- [ ] Controller Ä‘Ã£ inject IRepository vÃ  IMapper
- [ ] GET endpoints cÃ³ [AllowAnonymous]
- [ ] Migration thÃ nh cÃ´ng

---

## RULE 3: Cáº­p nháº­t file feature

AI pháº£i táº¡o file `../features/{STT}-{EntityName}.md` vá»›i ná»™i dung:

# Feature: {EntityName} Management

## NgÃ y táº¡o
YYYY-MM-DD

## MÃ´ táº£
API quáº£n lÃ½ {EntityName} vá»›i cÃ¡c chá»©c nÄƒng CRUD cÆ¡ báº£n.

## CÃ¡c file Ä‘Ã£ táº¡o

### Domain Layer
- `src/FlashOffer.API.Domain/Entities/{EntityName}.cs`

### Application Layer - DTOs
- `src/FlashOffer.API.Application/DTOs/{EntityName}Dto.cs`
- `src/FlashOffer.API.Application/DTOs/Create{EntityName}Dto.cs`
- `src/FlashOffer.API.Application/DTOs/Update{EntityName}Dto.cs`

### Application Layer - Validators
- `src/FlashOffer.API.Application/Validators/{EntityName}Validators.cs`

### WebApi Layer
- `src/FlashOffer.API.WebApi/Controllers/v1/{EntityName}Controller.cs`

## CÃ¡c file Ä‘Ã£ sá»­a

- `src/FlashOffer.API.Application/Mappings/MappingProfile.cs` - ThÃªm 3 mapping
- `src/FlashOffer.API.Infrastructure/Data/ApplicationDbContext.cs` - ThÃªm DbSet

## Migration
- Migration name: `Add{EntityName}Table`
- Table name: `{EntityName}s`

## API Endpoints

| Method | Endpoint                      | Auth         |
|--------|-------------------------------|--------------|
| GET    | `/api/v1/{entity-lower}`      | None         |
| GET    | `/api/v1/{entity-lower}/{id}` | None         |
| POST   | `/api/v1/{entity-lower}`      | JWT Required |
| PUT    | `/api/v1/{entity-lower}/{id}` | JWT Required |
| DELETE | `/api/v1/{entity-lower}/{id}` | JWT Required |

## Ghi chÃº

- ÄÃ£ cÃ³ soft delete (IsDeleted filter)
- ÄÃ£ cÃ³ auto timestamp (CreatedAt, UpdatedAt)

---

## RULE 4: Cáº­p nháº­t file features/README.md

AI pháº£i thÃªm dÃ²ng má»›i vÃ o báº£ng trong `../features/README.md`:

| {STT} | {EntityName} Management | [{STT}-{entity-lower}.md](./{STT}-{entity-lower}.md) | YYYY-MM-DD | âœ… Hoáº¡t Ä‘á»™ng |

---

## RULE 5: Kiá»ƒm tra tÃ­nh toÃ n váº¹n

AI pháº£i cháº¡y `dotnet build` vÃ  bÃ¡o cÃ¡o káº¿t quáº£.

---

## RULE 6: Commit thay Ä‘á»•i

AI pháº£i cháº¡y:

```bash
git add .
git commit -m "feat: add {EntityName} management API

- Added {EntityName} entity, DTOs, validator, controller
- Updated MappingProfile and DbContext
- Added migration Add{EntityName}Table
- Added feature documentation"
git push
```

---

## RULE 7: Docs sync vÃ  xÃ¡c nháº­n

Khi cÃ³ thay Ä‘á»•i áº£nh hÆ°á»Ÿng Ä‘áº¿n cáº¥u trÃºc, template hoáº·c quy trÃ¬nh khá»Ÿi táº¡o cá»§a repo, AI pháº£i:

1. Äáº£m báº£o cáº­p nháº­t tÃ i liá»‡u phÃ¹ há»£p trong `docs/FlashOffer.API.Documentation/` hoáº·c `README.md`.
2. Náº¿u thay Ä‘á»•i scripts bootstrap, prompt template, cáº¥u hÃ¬nh mÃ´i trÆ°á»ng hoáº·c layer core, thÃªm ghi chÃº rÃµ rÃ ng vÃ o docs.
3. Cháº¡y `scripts/validate-docs-sync.ps1` Ä‘á»ƒ kiá»ƒm tra ráº±ng náº¿u cÃ³ thay Ä‘á»•i cáº¥u trÃºc thÃ¬ docs cÅ©ng Ä‘Æ°á»£c cáº­p nháº­t.
4. BÃ¡o cÃ¡o rÃµ pháº§n docs Ä‘Ã£ cáº­p nháº­t trong pháº§n â€œGhi chÃºâ€ cá»§a bÃ¡o cÃ¡o thay Ä‘á»•i.

VÃ­ dá»¥:
- ÄÃ£ cáº­p nháº­t `docs/FlashOffer.API.Documentation/guides/14-project-bootstrap.md` khi thÃªm `scripts/init-template.ps1`.
- ÄÃ£ cáº­p nháº­t `docs/FlashOffer.API.Documentation/Prompts/01-Example-Prompt.md` khi má»Ÿ rá»™ng template AI.
