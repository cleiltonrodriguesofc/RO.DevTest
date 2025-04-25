// using RO.DevTest.Application.DTOs.Product.DeleteOrSearch;
// using RO.DevTest.Application.Interfaces.Repositories;
// using RO.DevTest.Domain.Entities;
// using System.Threading;
// using System.Threading.Tasks;

// namespace RO.DevTest.Application.UseCases.Product.Search
// {
//     public class SearchProductHandler : ISearchProductHandler
//     {
//         private readonly IProductRepository _productRepository;

//         public SearchProductHandler(IProductRepository productRepository)
//         {
//             _productRepository = productRepository;
//         }

//         public async Task<Product> Handle(ProductDeleteOrSearchRequest request, CancellationToken cancellationToken)
//         {
//             if (request.Id != null)
//             {
//                 return await _productRepository.GetByIdAsync(request.Id.Value);
//             }

//             if (!string.IsNullOrEmpty(request.Code))
//             {
//                 return await _productRepository.GetByCodeAsync(request.Code);
//             }

//             throw new Exception("No search criteria provided");
//         }
//     }
// }
