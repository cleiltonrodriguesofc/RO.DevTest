using Microsoft.EntityFrameworkCore;
using RO.DevTest.Application.Interfaces.Repositories;
using RO.DevTest.Domain.Entities;
using RO.DevTest.Persistence;

namespace RO.DevTest.Persistence.Repositories;

// repository to persist customer data
public class CustomerRepository : ICustomerRepository
{
    private readonly DefaultContext _context;

    // inject db context
    public CustomerRepository(DefaultContext context)
    {
        _context = context;
    }

    // add a new customer to the db
    public async Task AddAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
    }

    // check if a customer email already exists
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Customers.AnyAsync(c => c.Email == email);
    }

    // get all customers 
    public async Task<IEnumerable<Customer>> GetAllAsync()
        => await _context.Customers.ToListAsync();
    // get customer by id
    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customers.FindAsync(id);
    }
    // update customer
    public async Task UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    // delete customer by id, return true if found & removed
    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) 
            return false;

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return true;
    }

}
