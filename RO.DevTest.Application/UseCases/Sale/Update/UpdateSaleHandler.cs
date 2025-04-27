using RO.DevTest.Application.DTOs.Sale;
using RO.DevTest.Application.Interfaces.Repositories;
using RO.DevTest.Application.Interfaces.UseCases.Sale;
using RO.DevTest.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

// alias for sale class
using SaleEntity = RO.DevTest.Domain.Entities.Sale;

namespace RO.DevTest.Application.UseCases.Sale.Update
{
    public class UpdateSaleHandler : IUpdateSaleHandler
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;

        public UpdateSaleHandler(ISaleRepository saleRepository, IProductRepository productRepository)
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
        }

        public async Task<bool> ExecuteAsync(SaleUpdateRequest request)
        {
            // get the existing sale from database
            var sale = await _saleRepository.GetByIdAsync(request.Id);

            // if not found, return false
            if (sale == null)
                return false;

            // update the sale's customer id and date if needed
            sale.CustomerId = request.CustomerId;
            sale.CreatedAt = request.CreatedAt;

            // loop through the sale items and update them
            foreach (var item in request.Items)
            {
                // find the existing sale item
                var existingItem = sale.Items.FirstOrDefault(i => i.ProductId == item.ProductId);

                // if the item exists, update it
                if (existingItem != null)
                {
                    // validate if product exists
                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    if (product == null)
                        throw new Exception("product not found");

                    // validate stock availability
                    if (product.StockQuantity < item.Quantity)
                        throw new Exception("insufficient stock quantity");

                    // update the existing sale item
                    existingItem.Price = item.Price;
                    existingItem.Quantity = item.Quantity;

                    // update product stock
                    product.StockQuantity -= item.Quantity;
                    await _productRepository.UpdateAsync(product);
                }
                else
                {
                    // if item does not exist in the sale, create a new sale item
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

                    // create new sale item and add it to sale
                    var saleItem = new SaleItem
                    {
                        ProductId = item.ProductId,
                        Price = item.Price,
                        Quantity = item.Quantity
                    };

                    sale.Items.Add(saleItem);
                }
            }

            // recalculate total amount
            sale.TotalAmount = sale.Items.Sum(i => i.Price * i.Quantity);

            // save the updated sale
            await _saleRepository.UpdateAsync(sale);

            return true;
        }
    }
}
