using RO.DevTest.Application.DTOs.Customer;

namespace RO.DevTest.Application.Interfaces.UseCases.Customer;

// interface for creating a new customer
public interface ICreateCustomerHandler
{
    // executes the use case and returns the id of the newly created customer
    Task<int> ExecuteAsync(CustomerCreateRequest request);
}
