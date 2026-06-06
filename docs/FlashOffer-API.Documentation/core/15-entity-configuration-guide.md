# Hướng dẫn Cấu hình Entity (Entity Configuration)

Thay vì cấu hình trực tiếp trong `ApplicationDbContext`, mỗi Entity nên có một file cấu hình riêng sử dụng `IEntityTypeConfiguration`.

## 1. Quy tắc đặt tên
File cấu hình phải được đặt trong thư mục: `src/FlashOffer-API.Infrastructure/Data/Configurations/`
Tên file: `{EntityName}Configuration.cs`

## 2. Ví dụ ProductConfiguration.cs

```csharp
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Price).HasPrecision(18, 2);
    }
}
```

## 3. Cách hoạt động
Trong `ApplicationDbContext.cs`, chúng ta sử dụng phương thức sau để tự động đăng ký tất cả cấu hình:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}
```