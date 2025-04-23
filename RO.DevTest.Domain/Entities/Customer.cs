namespace RO.DevTest.Domain.Entities;

public class Customer
{
    // unique identifier for the customer
    public int Id { get; set; }

    // full name of the customer
    public string Name { get; set; } = null!;

    // email address of the customer
    public string Email { get; set; } = null!;

    // physical address of the customer
    public string Address { get; set; } = null!;

    // timestamp when the customer was created
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
