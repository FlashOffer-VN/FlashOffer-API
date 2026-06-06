```markdown
# Migration Template

## Commands

```bash
# Create migration
dotnet ef migrations add Add{EntityName}Table --project src/FlashOffer.API.Infrastructure --startup-project src/FlashOffer.API.WebApi

# Update database
dotnet ef database update --project src/FlashOffer.API.Infrastructure --startup-project src/FlashOffer.API.WebApi
```

## Migration code example (auto-generated)

```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.CreateTable(
        name: "{EntityName}s",
        columns: table => new
        {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
            UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            IsDeleted = table.Column<bool>(type: "bit", nullable: false)
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_{EntityName}s", x => x.Id);
        });
}
```