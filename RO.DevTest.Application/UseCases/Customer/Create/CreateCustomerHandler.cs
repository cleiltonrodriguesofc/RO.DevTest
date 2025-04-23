using RO.DevTest.Application.DTOs.Customer;
using RO.DevTest.Application.Interfaces.Repositories;
using RO.DevTest.Application.Interfaces.UseCases.Customer;
using RO.DevTest.Domain.Entities;

using CustomerEntity = RO.DevTest.Domain.Entities.Customer;

namespace RO.DevTest.Application.UseCases.Customer.Create;

// handles the creation of a new customer
public class CreateCustomerHandler : ICreateCustomerHandler
{
    private readonly ICustomerRepository _repository;

    public CreateCustomerHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> ExecuteAsync(CustomerCreateRequest request)
    {
        // check if email already exists
        var emailExists = await _repository.EmailExistsAsync(request.Email);
        if (emailExists)
            throw new InvalidOperationException("email already in use");

        // create new customer entity
        var customer = new CustomerEntity
        {
            Name = request.Name,
            Email = request.Email,
            Address = request.Address,
            CreatedAt = DateTime.UtcNow
        };

        // persist the customer
        await _repository.AddAsync(customer);

        // return id
        return customer.Id;
    }
}
