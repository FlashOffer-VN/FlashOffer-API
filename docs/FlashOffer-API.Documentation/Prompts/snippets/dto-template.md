# DTO Template

## Response DTO

namespace FlashOffer-API.Application.DTOs;

public class {EntityName}Dto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

## Create DTO

public class Create{EntityName}Dto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

## Update DTO

public class Update{EntityName}Dto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
