using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.Interfaces.Repositories;

// interface for sale repository
public interface ISaleRepository
{
    // get a sale by id
    Task<Sale?> GetByIdAsync(int id);

    // get all sales
    Task<IEnumerable<Sale>> GetAllAsync();

    // add a new sale
    Task AddAsync(Sale sale);

    // delete a sale by id
    Task<bool> DeleteAsync(int id);

    // update a sale
    Task UpdateAsync(Sale sale);
}
