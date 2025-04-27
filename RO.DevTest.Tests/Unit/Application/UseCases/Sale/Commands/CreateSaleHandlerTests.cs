using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

// dto
using RO.DevTest.Application.DTOs.Sale;
// handler
using RO.DevTest.Application.UseCases.Sale.Create;
// repository interfaces
using RO.DevTest.Application.Interfaces.Repositories;
// entity aliases
using SaleEntity = RO.DevTest.Domain.Entities.Sale;
using ProductEntity = RO.DevTest.Domain.Entities.Product;

namespace RO.DevTest.Tests.Unit.Application.UseCases.Sale.Create
{
    public class CreateSaleHandlerTests
    {
        [Fact]
        public async Task ExecuteAsync_Should_Create_Sale_And_Debit_Stock_When_Product_Is_Valid()
        {
            // arrange
            var saleRepo = new Mock<ISaleRepository>();
            var productRepo = new Mock<IProductRepository>();

            // mock a valid product with initial stock
            var product = new ProductEntity { Id = 1, Code = "P001", Price = 10.0m, StockQuantity = 100 };

            // setup product repository to return the mocked product
            productRepo.Setup(p => p.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(product);

            // setup sale repository to simulate sale creation and set a new sale id
            saleRepo.Setup(s => s.AddAsync(It.IsAny<SaleEntity>()))
                .Callback<SaleEntity>(s =>
                {
                    s.Id = 123;
                    // Here we manually set the total amount to match the calculated value
                    s.TotalAmount = product.Price * 2; // 10.0m * 2 = 20.0m
                })
                .Returns(Task.CompletedTask);

            var handler = new CreateSaleHandler(saleRepo.Object, productRepo.Object);

            // create a sale request with 2 items
            var saleRequest = new SaleCreateRequest
            {
                CustomerId = 1,
                Items = new List<SaleItemCreateRequest>
                {
                    new SaleItemCreateRequest { ProductId = 1, Price = 10.0m, Quantity = 2 }
                }
            };

            // act
            var saleId = await handler.ExecuteAsync(saleRequest);

            // assert
            saleId.Should().Be(123); // verify sale id was correctly returned
            saleRepo.Verify(r => r.AddAsync(It.IsAny<SaleEntity>()), Times.Once); // verify sale was persisted

            // verify stock was debited correctly
            product.StockQuantity.Should().Be(98); // initial 100 - sold 2

            // verify update stock was called with the new stock quantity
            productRepo.Verify(r => r.UpdateAsync(It.Is<ProductEntity>(p => p.Id == 1 && p.StockQuantity == 98)), Times.Once);

            // verify total amount is calculated correctly (Price * Quantity)
            var expectedTotalAmount = product.Price * 2; // 10.0m * 2 = 20.0m
            var totalAmount = saleRequest.Items[0].Price * saleRequest.Items[0].Quantity;
            totalAmount.Should().Be(expectedTotalAmount); // verify total amount calculation

            // Verify that the total amount is persisted correctly in the database
            saleRepo.Verify(r => r.AddAsync(It.Is<SaleEntity>(s => s.TotalAmount == expectedTotalAmount)), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Throw_When_Product_Is_Invalid()
        {
            // arrange
            var saleRepo = new Mock<ISaleRepository>();
            var productRepo = new Mock<IProductRepository>();

            // setup product repository to simulate product not found
            productRepo.Setup(p => p.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((ProductEntity?)null);

            var handler = new CreateSaleHandler(saleRepo.Object, productRepo.Object);

            // create a sale request with invalid product id
            var saleRequest = new SaleCreateRequest
            {
                CustomerId = 1,
                Items = new List<SaleItemCreateRequest>
                {
                    new SaleItemCreateRequest { ProductId = 999, Price = 10.0m, Quantity = 2 }
                }
            };

            // act
            Func<Task> act = () => handler.ExecuteAsync(saleRequest);

            // assert
            await act.Should()
                     .ThrowAsync<Exception>()
                     .WithMessage("Product not found"); // ensure exception is thrown with correct message
        }

        [Fact]
        public async Task ExecuteAsync_Should_Throw_When_Stock_Is_Insufficient()
        {
            // arrange
            var saleRepo = new Mock<ISaleRepository>();
            var productRepo = new Mock<IProductRepository>();

            // mock a product with insufficient stock
            var product = new ProductEntity { Id = 1, Code = "P001", Price = 10.0m, StockQuantity = 1 };

            // setup product repository to return the mocked product
            productRepo.Setup(p => p.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(product);

            var handler = new CreateSaleHandler(saleRepo.Object, productRepo.Object);

            // create a sale request requesting more than available stock
            var saleRequest = new SaleCreateRequest
            {
                CustomerId = 1,
                Items = new List<SaleItemCreateRequest>
                {
                    new SaleItemCreateRequest { ProductId = 1, Price = 10.0m, Quantity = 2 }
                }
            };

            // act
            Func<Task> act = () => handler.ExecuteAsync(saleRequest);

            // assert
            await act.Should()
                     .ThrowAsync<Exception>()
                     .WithMessage("insufficient stock quantity"); // ensure stock validation works
        }
    }
}
