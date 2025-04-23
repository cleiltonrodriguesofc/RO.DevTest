using RO.DevTest.Application.DTOs.Customer;
using RO.DevTest.Application.Interfaces.Repositories;
using RO.DevTest.Application.Interfaces.UseCases.Customer;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.UseCases.Customer.Update
{
    // handles updating an existing customer
    public class UpdateCustomerHandler : IUpdateCustomerHandler
    {
        private readonly ICustomerRepository _repository;

        // inject the repository
        public UpdateCustomerHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> HandleAsync(CustomerUpdateRequest request)
        {
            // get the customer from database
            var customer = await _repository.GetByIdAsync(request.Id);

            // if not found, return false
            if (customer == null)
                return false;

            // update the customer fields
            customer.Name = request.Name;
            customer.Email = request.Email;
            customer.Address = request.Address;

            // save the changes
            await _repository.UpdateAsync(customer);

            return true;
        }
    }
}
