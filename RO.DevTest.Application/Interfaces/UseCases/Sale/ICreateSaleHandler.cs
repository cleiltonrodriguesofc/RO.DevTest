using RO.DevTest.Application.DTOs.Sale;
using System.Threading.Tasks;

namespace RO.DevTest.Application.Interfaces.UseCases.Sale;

// interface for the sale creation handler
public interface ICreateSaleHandler
{
    // method to execute the sale creation
    Task<int> ExecuteAsync(SaleCreateRequest request);
}
