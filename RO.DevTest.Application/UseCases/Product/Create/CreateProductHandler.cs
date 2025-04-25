using RO.DevTest.Application.DTOs.Product;
using RO.DevTest.Application.Interfaces.Repositories;
using RO.DevTest.Application.Interfaces.UseCases.Product;
using RO.DevTest.Domain.Entities;
using System;
using System.Threading.Tasks;



using ProductEntity = RO.DevTest.Domain.Entities.Product;


namespace RO.DevTest.Application.UseCases.Product.Create;

// handler for creating a new product
public class CreateProductHandler : ICreateProductHandler
{
    private readonly IProductRepository _productRepository;

    public CreateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<int> ExecuteAsync(ProductCreateRequest request)
    {
        // check if code already exists
        var existing = await _productRepository.GetByCodeAsync(request.Code);
        if (existing != null)
        {
            throw new Exception("product code already exists");
        }

        // map dto to entity
        var product = new ProductEntity
        {
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            CreatedAt = DateTime.UtcNow

        };

        // persist to database
        await _productRepository.AddAsync(product);

        // return generated id
        return product.Id;
    }
}

