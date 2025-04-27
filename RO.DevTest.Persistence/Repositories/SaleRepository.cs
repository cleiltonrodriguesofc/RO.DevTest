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

    // get a sale by id
    public async Task<Sale?> GetByIdAsync(int id)
    {
        return await _context.Sales
            .Include(s => s.Items) // include sale items
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    // get all sales
    public async Task<IEnumerable<Sale>> GetAllAsync()
    {
        return await _context.Sales
            .Include(s => s.Items) // include sale items
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
}
