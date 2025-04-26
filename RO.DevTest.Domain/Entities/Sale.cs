namespace RO.DevTest.Domain.Entities;

// entity representing a sale
public class Sale
{
    public int Id { get; set; } // primary key
    public int CustomerId { get; set; }  // foreign key to customer
    public DateTime CreatedAt { get; set; }  // timestamp of sale creation
    public decimal TotalAmount { get; set; }   // sum of all sale items

    // navigation properties
    public Customer Customer { get; set; } = null!; // link to customer
    public List<SaleItem> Items { get; set; } = new(); // the items in this sale
}
