namespace RO.DevTest.Application.DTOs.Product;

// dto to create a new product
public class ProductCreateRequest
{
    public string Code { get; set; } = null!; // product code (unique)
    public string Name { get; set; } = null!; // product name
    public string Description { get; set; } = null!; // short description
    public decimal Price { get; set; } // product price
    public int StockQuantity { get; set; } // quantity available in stock
}
