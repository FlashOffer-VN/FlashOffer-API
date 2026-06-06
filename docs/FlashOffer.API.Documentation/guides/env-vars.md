# Biáº¿n mÃ´i trÆ°á»ng

## Má»¥c Ä‘Ã­ch

TÃ i liá»‡u nÃ y mÃ´ táº£ cÃ¡c biáº¿n mÃ´i trÆ°á»ng Ä‘Æ°á»£c sá»­ dá»¥ng bá»Ÿi project vÃ  cÃ¡ch config chÃºng.

## CÃ¡c file máº«u

- `.env.example`
- `.env.docker.example`

## Quy táº¯c chung

- `.env` vÃ  `.env.*` bá»‹ ignore bá»Ÿi Git Ä‘á»ƒ khÃ´ng commit secrets.
- `.env.example` vÃ  `.env.docker.example` Ä‘Æ°á»£c giá»¯ trong repo lÃ m máº«u.
- `DotNetEnv` Ä‘Æ°á»£c dÃ¹ng trong `Program.cs` Ä‘á»ƒ táº£i giÃ¡ trá»‹ tá»« `.env`.
- Environment variables cÅ©ng cÃ³ thá»ƒ Ä‘Æ°á»£c supply trá»±c tiáº¿p vÃ o há»‡ thá»‘ng.

## Biáº¿n báº¯t buá»™c

| Biáº¿n | Ã nghÄ©a | VÃ­ dá»¥ |
|------|---------|------|
| `DB_CONNECTION_STRING` | Chuá»—i káº¿t ná»‘i SQL Server | `Server=sql-server;Database=FlashOffer.APIDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;` |
| `JWT_SECRET` | KhÃ³a bÃ­ máº­t JWT (Ã­t nháº¥t 32 kÃ½ tá»±) | `YourSuperSecretKeyHereAtLeast32CharactersLong!` |
| `JWT_ISSUER` | Issuer cho token JWT | `FlashOffer.API` |
| `JWT_AUDIENCE` | Audience cho token JWT | `FlashOffer.APIClient` |
| `JWT_EXPIRY_MINUTES` | Thá»i háº¡n token (phÃºt) | `60` |

## Biáº¿n tÃ¹y chá»n

| Biáº¿n | Ã nghÄ©a | Máº·c Ä‘á»‹nh |
|------|---------|--------|
| `LOG_LEVEL` | Má»©c log Serilog | `Information` |
| `SA_PASSWORD` | Máº­t kháº©u SA cho SQL Server | `YourStrong!Passw0rd` |
| `ACCEPT_EULA` | Cháº¥p nháº­n Ä‘iá»u khoáº£n SQL Server | `Y` |

## TÆ°Æ¡ng tÃ¡c vá»›i Docker Compose

- `docker-compose.yml` dÃ¹ng file `.env.docker` Ä‘á»ƒ load giÃ¡ trá»‹.
- Báº¡n cáº§n táº¡o file tá»« máº«u:

```bash
cp .env.docker.example .env.docker
```

- Sau Ä‘Ã³ sá»­a giÃ¡ trá»‹ phÃ¹ há»£p.

## LÆ°u Ã½ báº£o máº­t

- KhÃ´ng commit `.env.docker` hoáº·c `.env` thá»±c táº¿.
- DÃ¹ng secret manager khi deploy production.
