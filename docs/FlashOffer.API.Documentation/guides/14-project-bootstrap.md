# Project bootstrap

## Má»¥c Ä‘Ã­ch

File nÃ y hÆ°á»›ng dáº«n cÃ¡ch dÃ¹ng `dotnet-api-base` nhÆ° má»™t starter template Ä‘á»ƒ khá»Ÿi táº¡o API má»›i nhanh chÃ³ng.

## Khi nÃ o dÃ¹ng

- Khi báº¡n muá»‘n táº¡o dá»± Ã¡n API má»›i mÃ  khÃ´ng pháº£i cáº¥u hÃ¬nh láº¡i cÃ¡c pháº§n cÆ¡ báº£n.
- Khi cáº§n giá»¯ láº¡i kiáº¿n trÃºc Clean Architecture, DI, validation, JWT, logging.
- Khi muá»‘n sá»­ dá»¥ng láº¡i template vá»›i Ã­t bÆ°á»›c setup nháº¥t.

## Báº¯t Ä‘áº§u nhanh báº±ng script

### 1. Clone repo

```bash
git clone https://github.com/TEN_CUA_BAN/dotnet-api-base.git TenDuAnMoi
cd TenDuAnMoi
```

### 2. Äá»•i tÃªn project tá»± Ä‘á»™ng

Cháº¡y script Ä‘á»•i tÃªn Ä‘á»ƒ cáº­p nháº­t folder, file vÃ  ná»™i dung:

```powershell
.\scripts\rename-project.ps1 -NewProjectName TenDuAnMoi
```

### 3. Khá»Ÿi táº¡o template

Tiáº¿p theo, táº¡o file cáº¥u hÃ¬nh `.env` náº¿u cáº§n vÃ  restore + build:

```powershell
.\scripts\init-template.ps1
```

Náº¿u thay Ä‘á»•i cáº¥u trÃºc repo hoáº·c scripts bootstrap, nhá»› cháº¡y thÃªm:

```powershell
.\scripts\validate-docs-sync.ps1
```

Báº¡n cÃ³ thá»ƒ thÃªm tÃ¹y chá»n:

```powershell
.\scripts\init-template.ps1 -UseDockerEnv -RunTests
```

## Náº¿u muá»‘n giá»¯ Git history cÅ©

Náº¿u báº¡n khÃ´ng cáº§n giá»¯ lá»‹ch sá»­ Git cÅ©, xÃ³a thÆ° má»¥c `.git` vÃ  táº¡o repository má»›i:

```bash
rm -rf .git
git init
git add .
git commit -m "Initial commit from FlashOffer.API"
```

## Cáº¥u hÃ¬nh mÃ´i trÆ°á»ng

Náº¿u báº¡n dÃ¹ng file `.env.example`, `init-template.ps1` sáº½ sao chÃ©p nÃ³ thÃ nh `.env` tá»± Ä‘á»™ng.

Náº¿u muá»‘n dÃ¹ng Docker env:

```powershell
.\scripts\init-template.ps1 -UseDockerEnv
```

## Kiá»ƒm tra project

- Cháº¡y restore vÃ  build

```bash
dotnet restore TenDuAnMoi.slnx
dotnet build TenDuAnMoi.slnx
```

- Cháº¡y API

```bash
cd src\TenDuAnMoi.WebApi
dotnet run
```

## Checklist template

- [ ] Äá»•i tÃªn solution vÃ  project folder
- [ ] Äá»•i tÃªn csproj vÃ  namespace trong ná»™i dung file
- [ ] Táº¡o file `.env` tá»« `.env.example`
- [ ] Build solution
- [ ] Cháº¡y test náº¿u cáº§n
- [ ] Cáº­p nháº­t `README.md` vÃ  docs sau khi rename

## Gá»£i Ã½

- Giá»¯ láº¡i cáº¥u trÃºc `src/Domain`, `src/Application`, `src/Infrastructure`, `src/WebApi`.
- Náº¿u dá»± Ã¡n má»›i khÃ´ng cáº§n `tests/`, báº¡n cÃ³ thá»ƒ xÃ³a cÃ¡c thÆ° má»¥c test.
- Náº¿u cáº§n Docker, copy `docker-compose.yml` vÃ  `.env.docker.example`.

## Tham kháº£o

- `README.md` pháº§n `CÃ¡ch tÃ¡i sá»­ dá»¥ng cho dá»± Ã¡n má»›i`
- `scripts/rename-project.ps1`
- `scripts/init-template.ps1`
