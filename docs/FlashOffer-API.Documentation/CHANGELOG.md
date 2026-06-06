# Changelog

All notable changes to this project will be documented in this file.

## [Unreleased]

### Added
- Initial project setup with Clean Architecture
- JWT Authentication support
- API Versioning (v1)
- FluentValidation integration
- AutoMapper for Entity-DTO mapping
- Serilog logging (console + file)
- Global Exception Middleware
- Standardized API Response (ApiResponse<T>)
- Docker support with docker-compose
- GitHub Actions CI/CD workflows
- Shared project for cross-layer utilities
- .NET 9 upgrade
- Environment variables support (.env)
- Documentation with AI rules and prompts
- Code snippets for entity, dto, controller, validator, automapper, repository, migration

### Changed
- Upgraded from .NET 8 to .NET 9
- Centralized package management with Directory.Packages.props

### Fixed
- Font encoding issues in documentation files
- Docker SQL Server connection issues

### Security
- JWT authentication with configurable secret
- Security rules documented in Prompts/Rules/security-rules.md
