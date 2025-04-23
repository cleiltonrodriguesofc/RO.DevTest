using RO.DevTest.Application.Interfaces.Repositories;
using RO.DevTest.Application.Interfaces.UseCases.Customer;

namespace RO.DevTest.Application.UseCases.Customer.Delete
{
    // handles the deletion of a customer
    public class DeleteCustomerHandler : IDeleteCustomerHandler
    {
        private readonly ICustomerRepository _repository;

        // inject repository to access data
        public DeleteCustomerHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> HandleAsync(int id)
        {
            // try to delete customer; repository returns if existed
            return await _repository.DeleteAsync(id);
        }
    }
}
