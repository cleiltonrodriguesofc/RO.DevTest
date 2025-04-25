using System.Collections.Generic;
using System.Threading.Tasks;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.Interfaces.Repositories;

// repository interface for product
public interface IProductRepository
{
    // // get product by id
    // Task<Product?> GetByIdAsync(int id);

    // // get product by code
    Task<Product?> GetByCodeAsync(string code);

    // get all products
    Task<IEnumerable<Product>> GetAllAsync();

    // add a new product
    Task AddAsync(Product product);

    // // update an existing product
    // Task UpdateAsync(Product product);

    // // delete a product
    // Task DeleteAsync(Product product);
}

