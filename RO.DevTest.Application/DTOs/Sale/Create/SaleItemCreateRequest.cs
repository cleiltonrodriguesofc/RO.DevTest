namespace RO.DevTest.Application.DTOs.Sale;

// dto for creating a sale item
public class SaleItemCreateRequest
{
    public int ProductId { get; set; } // product id
    public decimal Price { get; set; } // unit price at the time of sale
    public int Quantity { get; set; } // quantity sold
}
