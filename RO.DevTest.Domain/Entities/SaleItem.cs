namespace RO.DevTest.Domain.Entities;

// entity representing one item in a sale
public class SaleItem
{
    public int Id { get; set; } // primary key
    public int SaleId { get; set; } // foreign key to sale
    public int ProductId { get; set; } // foreign key to product
    public decimal Price { get; set; } // unit price at time of sale
    public int Quantity { get; set; }  // quantity sold

    // navigation properties
    public Sale Sale { get; set; } = null!;   // link back to sale
    public Product Product { get; set; } = null!;  // link to product
}
