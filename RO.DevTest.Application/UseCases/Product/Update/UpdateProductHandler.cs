// using RO.DevTest.Application.DTOs.Product.Update;
// using RO.DevTest.Application.Interfaces.Repositories;
// using RO.DevTest.Domain.Entities;
// using System.Threading;
// using System.Threading.Tasks;

// namespace RO.DevTest.Application.UseCases.Product.Update
// {
//     public class UpdateProductHandler : IUpdateProductHandler
//     {
//         private readonly IProductRepository _productRepository;

//         public UpdateProductHandler(IProductRepository productRepository)
//         {
//             _productRepository = productRepository;
//         }

//         public async Task<bool> Handle(ProductUpdateRequest request, CancellationToken cancellationToken)
//         {
//             var existingProduct = await _productRepository.GetByIdAsync(request.Id);
//             if (existingProduct == null)
//             {
//                 throw new Exception("Product not found");
//             }

//             // update the product
//             existingProduct.Code = request.Code;
//             existingProduct.Name = request.Name;
//             existingProduct.Description = request.Description;
//             existingProduct.Price = request.Price;
//             existingProduct.StockQuantity = request.StockQuantity;

//             await _productRepository.UpdateAsync(existingProduct);
//             return true; // return success
//         }
//     }
// }
