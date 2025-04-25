using RO.DevTest.Application.DTOs.Product;
using RO.DevTest.Application.Interfaces.Repositories;
using RO.DevTest.Application.Interfaces.UseCases.Product;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.UseCases.Product.Update
{
    // handles updating an existing product
    public class UpdateProductHandler : IUpdateProductHandler
    {
        private readonly IProductRepository _repository;

        // inject the repository
        public UpdateProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> HandleAsync(ProductUpdateRequest request)
        {
            // get the product from database
            var product = await _repository.GetByIdAsync(request.Id);

            // if not found, return false
            if (product == null)
                return false;

            // update the product fields
            product.Code = request.Code;
            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.StockQuantity = request.StockQuantity;

            // save the changes
            await _repository.UpdateAsync(product);

            return true;
        }
    }
}
