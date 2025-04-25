using RO.DevTest.Application.DTOs.Product;

namespace RO.DevTest.Application.Interfaces.UseCases.Product;

// interface for creating a new product
public interface ICreateProductHandler
{
    // executes the use case and returns the id of the newly created product
    Task<int> ExecuteAsync(ProductCreateRequest request);
}
