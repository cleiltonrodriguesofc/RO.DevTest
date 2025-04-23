namespace RO.DevTest.Application.DTOs.Customer;

// dto to receive customer creation data
public class CustomerCreateRequest
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Address { get; set; } = null!;
}
