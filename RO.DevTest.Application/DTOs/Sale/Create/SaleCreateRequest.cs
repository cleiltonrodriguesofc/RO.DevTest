namespace RO.DevTest.Application.DTOs.Sale;

// dto for creating a new sale
public class SaleCreateRequest
{
    public int CustomerId { get; set; } // customer linked to the sale
    public List<SaleItemCreateRequest> Items { get; set; } = new(); // items of the sale
}
