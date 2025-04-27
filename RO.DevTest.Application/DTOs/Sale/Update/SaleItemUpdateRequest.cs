namespace RO.DevTest.Application.DTOs.Sale;

// dto for updating a sale item
public class SaleItemUpdateRequest
{
    public int ProductId { get; set; } // product id
    public decimal Price { get; set; } // updated unit price
    public int Quantity { get; set; } // updated quantity
}

