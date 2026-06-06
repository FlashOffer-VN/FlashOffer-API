using System;

namespace FlashOffer-API.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string Category { get; set; } = string.Empty;

    public void UpdateStock(int quantity)
    {
        if (quantity < 0 && Math.Abs(quantity) > StockQuantity)
            throw new InvalidOperationException("Not enough stock");

        StockQuantity += quantity;
    }

    public bool IsInStock() => StockQuantity > 0;
}
