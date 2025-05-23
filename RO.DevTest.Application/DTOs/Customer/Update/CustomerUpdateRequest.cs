namespace RO.DevTest.Application.DTOs.Customer;

public class CustomerUpdateRequest
{
    // get id to update
    public int Id { get; set; } 
    
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}
