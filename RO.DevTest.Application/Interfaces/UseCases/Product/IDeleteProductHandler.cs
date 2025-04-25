// interface to handle the update of a product
using RO.DevTest.Application.DTOs.Product;

namespace RO.DevTest.Application.Interfaces.UseCases.Product;

// handles deleting a product by id
public interface IDeleteProductHandler
{
    // delete product by id
    Task<bool> HandleAsync(int id);

}
