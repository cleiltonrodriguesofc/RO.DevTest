// interface to handle the update of a customer
using RO.DevTest.Application.DTOs.Customer;

namespace RO.DevTest.Application.Interfaces.UseCases.Customer;

// defines the contract for updating a customer
public interface IUpdateCustomerHandler
{
    // handle the update request and return true if successful
    Task<bool> HandleAsync(CustomerUpdateRequest request);
}
