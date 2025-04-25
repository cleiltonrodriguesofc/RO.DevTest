using RO.DevTest.Application.DTOs.Product;

namespace RO.DevTest.Application.Interfaces.UseCases.Product;

// defines the contract for product update handler
public interface IUpdateProductHandler
{
    Task<bool> HandleAsync(ProductUpdateRequest request);
}
