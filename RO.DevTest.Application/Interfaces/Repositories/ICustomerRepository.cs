using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.Interfaces.Repositories;

// abstraction for customer persistence
public interface ICustomerRepository
{
    Task AddAsync(Customer customer); 
    Task<bool> EmailExistsAsync(string email); //don't use duplicated email

    // get all Customers
    Task<IEnumerable<Customer>> GetAllAsync();

    // get customer by it
    Task<Customer?> GetByIdAsync(int id);
    // update customer
    Task UpdateAsync(Customer customer); 

    // delete customer by id
    Task<bool> DeleteAsync(int id);   
}
