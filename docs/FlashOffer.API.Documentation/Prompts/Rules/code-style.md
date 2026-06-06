# Code Style & Conventions

- Naming: PascalCase for types/methods, camelCase for parameters/local variables.
- Layering: Domain models only in `FlashOffer.API.Domain`; DTOs in `Application/DTOs`.
- Controllers: inherit `ApiControllerBase`; use injected `IRepository<T>` / services.
- Exception handling: dÃ¹ng `GlobalExceptionMiddleware` â€” khÃ´ng try/catch rá»™ng trong controller.
- DI lifetimes: repository/service -> `Scoped`; lightweight helper -> `Transient`; config singletons -> `Singleton`.
- Tests: unit tests for services, integration tests for API endpoints.
- Formatting: follow existing repo style; keep lines <= 120 chars where practical.
- Comments: prefer self-explanatory code; chá»‰ comment cho intent hoáº·c complex logic.
