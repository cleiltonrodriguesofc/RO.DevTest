using RO.DevTest.Application.DTOs.Sale;
using System.Threading.Tasks;

namespace RO.DevTest.Application.Interfaces.UseCases.Sale;

// interface for updating a sale
public interface IUpdateSaleHandler
{
    // execute update sale
    Task<bool> ExecuteAsync(SaleUpdateRequest request);
}

