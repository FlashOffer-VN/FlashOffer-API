# Kiến trúc dự án - Clean Architecture

## Tổng quan

Dự án sử dụng Clean Architecture với 4 lớp chính:

Request → WebApi → Application → Infrastructure → Database
           ↑           ↑              ↑
           └───────────┴──────────────┘
                  Dependency Inversion

## Các layer

| Layer          | Vai trò                                                    | Thư mục                                   | Phụ thuộc                              |
|----------------|------------------------------------------------------------|-------------------------------------------|----------------------------------------|
| Domain         | Entities, Value Objects, Enums, Business rules             | src/FlashOffer-API.Domain                  | Không phụ thuộc layer nào              |
| Application    | DTOs, Interfaces, Use Cases, Business logic                | src/FlashOffer-API.Application             | Chỉ phụ thuộc Domain                   |
| Infrastructure | DbContext, Repositories, External Services                 | src/FlashOffer-API.Infrastructure          | Phụ thuộc Application                  |
| WebApi         | Controllers, Middlewares, Filters                          | src/FlashOffer-API.WebApi                  | Phụ thuộc Application + Infrastructure |

## Nguyên tắc quan trọng

1. Domain layer không được biết đến các layer khác
2. Application layer chỉ biết Domain, không biết Infrastructure
3. Infrastructure triển khai các interface từ Application
4. WebApi chỉ orchestrate các dependency, không chứa business logic

## Dependency Injection

Mỗi layer có file DependencyInjection.cs riêng:

- Application/DependencyInjection.cs - Đăng ký services của Application layer
- Infrastructure/DependencyInjection.cs - Đăng ký services của Infrastructure layer
- WebApi/DependencyInjection.cs - Tổng hợp và đăng ký thêm API-specific services

## Flow xử lý request

1. Controller nhận request → gọi Service/Handler
2. Service/Handler gọi Repository (interface từ Application, impl từ Infrastructure)
3. Repository truy xuất database qua DbContext
4. Kết quả trả về qua Response wrapper (ApiResponse<T>)

## Ví dụ về dependency flow

WebApi.csproj tham chiếu → Application.csproj và Infrastructure.csproj
Application.csproj tham chiếu → Domain.csproj
Infrastructure.csproj tham chiếu → Application.csproj

## Tóm tắt

- Domain: Định nghĩa entity và business rule
- Application: Định nghĩa interface và DTO
- Infrastructure: Triển khai interface (DbContext, Repository)
- WebApi: Điều phối và trả về response