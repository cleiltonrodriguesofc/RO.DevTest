using RO.DevTest.Application.DTOs.Sale;
using System.Threading.Tasks;


// interface to handle the deletion of a sale
namespace RO.DevTest.Application.Interfaces.UseCases.Sale;

// handles deleting a sale by id
public interface IDeleteSaleHandler
{
    // delete sale by id
    Task<bool> HandleAsync(int id);
}
