# HÆ°á»›ng dáº«n Cáº¥u hÃ¬nh Entity (Entity Configuration)

Thay vÃ¬ cáº¥u hÃ¬nh trá»±c tiáº¿p trong `ApplicationDbContext`, má»—i Entity nÃªn cÃ³ má»™t file cáº¥u hÃ¬nh riÃªng sá»­ dá»¥ng `IEntityTypeConfiguration`.

## 1. Quy táº¯c Ä‘áº·t tÃªn
File cáº¥u hÃ¬nh pháº£i Ä‘Æ°á»£c Ä‘áº·t trong thÆ° má»¥c: `src/FlashOffer.API.Infrastructure/Data/Configurations/`
TÃªn file: `{EntityName}Configuration.cs`

## 2. VÃ­ dá»¥ ProductConfiguration.cs

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

## 3. CÃ¡ch hoáº¡t Ä‘á»™ng
Trong `ApplicationDbContext.cs`, chÃºng ta sá»­ dá»¥ng phÆ°Æ¡ng thá»©c sau Ä‘á»ƒ tá»± Ä‘á»™ng Ä‘Äƒng kÃ½ táº¥t cáº£ cáº¥u hÃ¬nh:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}
```