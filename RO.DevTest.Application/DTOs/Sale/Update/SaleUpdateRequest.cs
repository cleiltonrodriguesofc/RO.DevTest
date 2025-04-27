using System.Collections.Generic;

namespace RO.DevTest.Application.DTOs.Sale;

// dto for updating a sale
public class SaleUpdateRequest
{
    // sale id to be updated
    public int Id { get; set; }

    // customer id associated with the sale
    public int CustomerId { get; set; }

    public DateTime CreatedAt { get; set; }

    // list of sale items to be updated
    public List<SaleItemUpdateRequest> Items { get; set; } = new List<SaleItemUpdateRequest>();
}
