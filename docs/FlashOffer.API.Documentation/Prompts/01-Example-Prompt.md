# Prompt template cho AI

## MÃ´ táº£
ÄÃ¢y lÃ  template prompt dÃ¹ng cho AI khi thÃªm feature CRUD má»›i vÃ o dá»± Ã¡n `dotnet-api-base`.

## NguyÃªn táº¯c chÃ­nh
- LuÃ´n Æ°u tiÃªn `docs/FlashOffer.API.Documentation/` náº¿u ná»™i dung local khÃ¡c vá»›i kiáº¿n thá»©c chung.
- Dá»± Ã¡n sá»­ dá»¥ng Clean Architecture, AutoMapper, FluentValidation, soft delete vÃ  chuáº©n `ApiResponse<T>`.
- Chá»‰ táº¡o/sá»­a file cáº§n thiáº¿t cho feature má»›i.
- TrÃ¡nh thay Ä‘á»•i cÃ¡c file quan trá»ng trá»« khi thá»±c sá»± cáº§n thiáº¿t.
- Build solution vÃ  kiá»ƒm tra compile sau khi hoÃ n thÃ nh.

## Format prompt
Sá»­ dá»¥ng cáº¥u trÃºc: 
- `Context:` mÃ´ táº£ ngáº¯n vá» repo
- `Task:` mÃ´ táº£ feature cáº§n táº¡o
- `Constraints:` cÃ¡c giá»›i háº¡n vÃ  quy táº¯c
- `Done when:` chá»‰ ra tiÃªu chÃ­ hoÃ n thÃ nh

## Máº«u prompt
Add Brand: Name*, Description (max 500)

## Checklist AI cáº§n thá»±c hiá»‡n
1. Äá»c `docs/FlashOffer.API.Documentation/` Ä‘á»ƒ náº¯m convention vÃ  rule.
2. XÃ¡c Ä‘á»‹nh cÃ´ng viá»‡c dá»±a trÃªn template thÃªm feature má»›i cá»§a repo.
3. Táº¡o cÃ¡c file sau:
   - `src/FlashOffer.API.Domain/Entities/{EntityName}.cs`
   - `src/FlashOffer.API.Infrastructure/Data/Configurations/{EntityName}Configuration.cs`
   - `src/FlashOffer.API.Application/DTOs/{EntityName}Dto.cs`, `Create{EntityName}Dto.cs`, `Update{EntityName}Dto.cs`
   - `src/FlashOffer.API.Application/Validators/{EntityName}Validators.cs`
   - `src/FlashOffer.API.WebApi/Controllers/v1/{EntityName}Controller.cs`
4. Äáº£m báº£o DTO sá»­ dá»¥ng `IMapFrom<T>` Ä‘á»ƒ AutoMapper tá»± nháº­n mapping.
5. ThÃªm `DbSet<{EntityName}>` vÃ o `src/FlashOffer.API.Infrastructure/Data/ApplicationDbContext.cs`.
6. Build solution Ä‘á»ƒ xÃ¡c nháº­n khÃ´ng cÃ³ lá»—i.
7. Náº¿u cÃ³ thay Ä‘á»•i liÃªn quan tá»›i cáº¥u trÃºc, template hoáº·c khá»Ÿi táº¡o repo, cháº¡y `scripts/validate-docs-sync.ps1` Ä‘á»ƒ kiá»ƒm tra Ä‘á»“ng bá»™ docs.
8. Tá»•ng há»£p cÃ¡c file Ä‘Ã£ táº¡o/sá»­a vÃ  tráº£ lá»i ngáº¯n gá»n.

## VÃ­ dá»¥ prompt cá»¥ thá»ƒ
> Context: `dotnet-api-base` lÃ  dá»± Ã¡n Clean Architecture cÃ³ docs local táº¡i `docs/FlashOffer.API.Documentation/`.
> Task: ThÃªm feature `Brand` vá»›i cÃ¡c trÆ°á»ng:
> - `Name` (required, max 200)
> - `Description` (optional, max 500)
> Constraints: KhÃ´ng sá»­a file quan trá»ng trá»« khi cáº§n, Æ°u tiÃªn local docs, pháº£i táº¡o entity/config/DTO/validator/controller/DbSet, build Ä‘á»ƒ kiá»ƒm tra.
> Done when: code compile thÃ nh cÃ´ng, endpoint CRUD tá»“n táº¡i, validation Ä‘Ãºng.

## LÆ°u Ã½ cáº£i thiá»‡n
- Náº¿u prompt chá»‰ cÃ³ ná»™i dung ngáº¯n nhÆ° `Add Brand: Name*, Description (max 500)`, AI váº«n pháº£i hiá»ƒu task lÃ  táº¡o feature CRUD Ä‘áº§y Ä‘á»§ theo convention cá»§a repo.
- Náº¿u local docs cÃ³ rule cá»¥ thá»ƒ, Æ°u tiÃªn Ã¡p dá»¥ng chÃºng hÆ¡n kiáº¿n thá»©c chung.
- Náº¿u tháº¥y model `Product` hoáº·c `SampleController`, hÃ£y dÃ¹ng nÃ³ lÃ m máº«u Ä‘á»‹nh dáº¡ng.

## Template repo bootstrap
Náº¿u cáº§n dÃ¹ng repo nÃ y nhÆ° starter template, AI nÃªn há»— trá»£ cÃ¡c bÆ°á»›c bootstrap sau:
- Giá»¯ nguyÃªn cáº¥u trÃºc `src/Domain`, `src/Application`, `src/Infrastructure`, `src/WebApi`.
- Cháº¡y `scripts/rename-project.ps1 -NewProjectName <TÃªnProject>` Ä‘á»ƒ Ä‘á»•i tÃªn project vÃ  namespace.
- Cháº¡y `scripts/init-template.ps1` Ä‘á»ƒ táº¡o file `.env`, restore packages vÃ  build solution.
- Náº¿u cáº§n Docker env, dÃ¹ng `scripts/init-template.ps1 -UseDockerEnv`.
- Náº¿u cáº§n kiá»ƒm tra toÃ n bá»™, dÃ¹ng `scripts/init-template.ps1 -RunTests`.

### VÃ­ dá»¥ prompt bootstrap
> Context: `dotnet-api-base` lÃ  starter repo template vá»›i scripts bootstrap.
> Task: Äá»•i tÃªn project thÃ nh `MyApiApp`, táº¡o `.env` tá»« `.env.example`, restore vÃ  build.
> Constraints: chá»‰ dÃ¹ng script cÃ³ sáºµn, khÃ´ng sá»­a logic core cá»§a repo.
> Done when: repo Ä‘Æ°á»£c Ä‘á»•i tÃªn, build thÃ nh cÃ´ng.

