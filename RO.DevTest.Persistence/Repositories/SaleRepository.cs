using Microsoft.EntityFrameworkCore;
using RO.DevTest.Application.Interfaces.Repositories;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Persistence.Repositories;

// repository for sale entity
public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    // get a sale by id, including customer and product details
    public async Task<Sale?> GetByIdAsync(int id)
    {
        return await _context.Sales
            .Include(s => s.Customer)                    // include customer
            .Include(s => s.Items)                       // include sale items
                .ThenInclude(i => i.Product)             // include product on each item
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    // get all sales, including customer and product details
    public async Task<IEnumerable<Sale>> GetAllAsync()
    {
        return await _context.Sales
            .Include(s => s.Customer)                    // include customer
            .Include(s => s.Items)                       // include sale items
                .ThenInclude(i => i.Product)             // include product on each item
            .ToListAsync();
    }

    // add a new sale
    public async Task AddAsync(Sale sale)
    {
        await _context.Sales.AddAsync(sale);
        await _context.SaveChangesAsync();
    }

    // delete a sale by id
    public async Task<bool> DeleteAsync(int id)
    {
        var sale = await _context.Sales.FindAsync(id);

        // return false if sale does not exist
        if (sale == null)
            return false;

        _context.Sales.Remove(sale);
        await _context.SaveChangesAsync();

        return true;
    }

    // update a sale
    public async Task UpdateAsync(Sale sale)
    {
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync();
    }
}
