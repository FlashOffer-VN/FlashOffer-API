# Security Rules

- NEVER include secrets, private keys, or production credentials in generated files.
- Use environment variables or `dotnet user-secrets` for secrets.
- Input validation: always validate incoming DTOs via FluentValidation.
- Authentication: use JWT as configured; do not bypass auth in generated controllers.
- Authorization: annotate endpoints with `[Authorize]` or `[AllowAnonymous]` as required.
- Logging: do not log secrets; redact sensitive fields.
- Dependencies: prefer vetted NuGet packages; avoid experimental packages without approval.
