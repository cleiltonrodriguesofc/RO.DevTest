using RO.DevTest.Application.DTOs.Sale;
using RO.DevTest.Application.Interfaces.Repositories;
using RO.DevTest.Application.Interfaces.UseCases.Sale;
using RO.DevTest.Domain.Entities;
using System;
using System.Threading.Tasks;
using System.Linq;

// alias for sale class
using SaleEntity = RO.DevTest.Domain.Entities.Sale;

namespace RO.DevTest.Application.UseCases.Sale.Create;

public class CreateSaleHandler : ICreateSaleHandler
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;

    public CreateSaleHandler(ISaleRepository saleRepository, IProductRepository productRepository)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
    }

    public async Task<int> ExecuteAsync(SaleCreateRequest request)
    {
        // create a new sale entity
        var sale = new SaleEntity
        {
            CustomerId = request.CustomerId,
            CreatedAt = DateTime.UtcNow
        };

        // loop through sale items
        foreach (var item in request.Items)
        {
            // get the product by id
            var product = await _productRepository.GetByIdAsync(item.ProductId);

            // validate if product exists
            if (product == null)
                throw new Exception("product not found");

            // validate stock availability
            if (product.StockQuantity < item.Quantity)
                throw new Exception("insufficient stock quantity");

            // debit product stock
            product.StockQuantity -= item.Quantity;
            await _productRepository.UpdateAsync(product);

            // create and add sale item
            var saleItem = new SaleItem
            {
                ProductId = item.ProductId,
                Price = item.Price,
                Quantity = item.Quantity
            };

            sale.Items.Add(saleItem);
        }

        // calculate total amount
        sale.TotalAmount = sale.Items.Sum(i => i.Price * i.Quantity);

        // save the sale
        await _saleRepository.AddAsync(sale);

        // return the id of the created sale
        return sale.Id;
    }
}
