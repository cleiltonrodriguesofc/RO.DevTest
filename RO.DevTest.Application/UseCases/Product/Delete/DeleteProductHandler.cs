// using RO.DevTest.Application.DTOs.Product.DeleteOrSearch;
// using RO.DevTest.Application.Interfaces.Repositories;
// using RO.DevTest.Application.Interfaces.UseCases.Product;
// using System.Threading;
// using System.Threading.Tasks;

// namespace RO.DevTest.Application.UseCases.Product.Delete
// {
//     public class DeleteProductHandler : IDeleteProductHandler
//     {
//         private readonly IProductRepository _productRepository;

//         public DeleteProductHandler(IProductRepository productRepository)
//         {
//             _productRepository = productRepository;
//         }

//         public async Task<bool> Handle(ProductDeleteOrSearchRequest request, CancellationToken cancellationToken)
//         {
//             if (request.Id != null)
//             {
//                 var product = await _productRepository.GetByIdAsync(request.Id.Value);
//                 if (product == null)
//                 {
//                     throw new Exception("Product not found");
//                 }

//                 await _productRepository.DeleteAsync(product);
//                 return true; // return success
//             }

//             if (!string.IsNullOrEmpty(request.Code))
//             {
//                 var product = await _productRepository.GetByCodeAsync(request.Code);
//                 if (product == null)
//                 {
//                     throw new Exception("Product not found");
//                 }

//                 await _productRepository.DeleteAsync(product);
//                 return true; // return success
//             }

//             throw new Exception("Product not specified for deletion");
//         }
//     }
// }
