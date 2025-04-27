using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

// dtos
using RO.DevTest.Application.DTOs.Sale;
// handler
using RO.DevTest.Application.UseCases.Sale.Update;
// repository interfaces
using RO.DevTest.Application.Interfaces.Repositories;
// entity aliases
using SaleEntity = RO.DevTest.Domain.Entities.Sale;
using ProductEntity = RO.DevTest.Domain.Entities.Product;

namespace RO.DevTest.Tests.Unit.Application.UseCases.Sale.Commands
{
    public class UpdateSaleHandlerTests
    {
        [Fact]
        public async Task ExecuteAsync_Should_Add_New_Item_And_Adjust_Stock_When_Sale_Exists()
        {
            // arrange
            var saleRepo = new Mock<ISaleRepository>();
            var productRepo = new Mock<IProductRepository>();

            // existing sale with no items
            var existingSale = new SaleEntity
            {
                Id = 1,
                CustomerId = 1,
                CreatedAt = DateTime.UtcNow,
                TotalAmount = 0m,
                Items = new List<RO.DevTest.Domain.Entities.SaleItem>()
            };
            saleRepo.Setup(r => r.GetByIdAsync(1))
                    .ReturnsAsync(existingSale);
            saleRepo.Setup(r => r.UpdateAsync(existingSale))
                    .Returns(Task.CompletedTask);

            // product to add
            var product = new ProductEntity { Id = 1, Code = "P001", Price = 10m, StockQuantity = 100 };
            productRepo.Setup(r => r.GetByIdAsync(1))
                       .ReturnsAsync(product);
            productRepo.Setup(r => r.UpdateAsync(product))
                       .Returns(Task.CompletedTask);

            var handler = new UpdateSaleHandler(saleRepo.Object, productRepo.Object);

            var request = new SaleUpdateRequest
            {
                Id = 1,
                CustomerId = 1,
                Items = new List<SaleItemUpdateRequest>
                {
                    new SaleItemUpdateRequest { ProductId = 1, Price = 10m, Quantity = 2 }
                }
            };

            // act
            var result = await handler.ExecuteAsync(request);

            // assert
            result.Should().BeTrue();
            existingSale.Items.Should().HaveCount(1);
            existingSale.Items[0].ProductId.Should().Be(1);
            existingSale.Items[0].Price.Should().Be(10m);
            existingSale.Items[0].Quantity.Should().Be(2);
            existingSale.TotalAmount.Should().Be(20m);
            product.StockQuantity.Should().Be(98);
            productRepo.Verify(r => r.UpdateAsync(
                It.Is<ProductEntity>(p => p.Id == 1 && p.StockQuantity == 98)
            ), Times.Once);
            saleRepo.Verify(r => r.UpdateAsync(existingSale), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Return_False_When_Sale_Does_Not_Exist()
        {
            // arrange
            var saleRepo = new Mock<ISaleRepository>();
            var productRepo = new Mock<IProductRepository>();

            saleRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync((SaleEntity)null!);

            var handler = new UpdateSaleHandler(saleRepo.Object, productRepo.Object);

            var request = new SaleUpdateRequest
            {
                Id = 99,
                CustomerId = 1,
                Items = new List<SaleItemUpdateRequest>()
            };

            // act
            var result = await handler.ExecuteAsync(request);

            // assert
            result.Should().BeFalse();
            saleRepo.Verify(r => r.UpdateAsync(It.IsAny<SaleEntity>()), Times.Never);
            productRepo.Verify(r => r.UpdateAsync(It.IsAny<ProductEntity>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Throw_When_Product_Not_Found()
        {
            // arrange
            var saleRepo = new Mock<ISaleRepository>();
            var productRepo = new Mock<IProductRepository>();

            var existingSale = new SaleEntity
            {
                Id = 1,
                CustomerId = 1,
                CreatedAt = DateTime.UtcNow,
                TotalAmount = 0m,
                Items = new List<RO.DevTest.Domain.Entities.SaleItem>()
            };
            saleRepo.Setup(r => r.GetByIdAsync(1))
                    .ReturnsAsync(existingSale);
            productRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                       .ReturnsAsync((ProductEntity)null!);

            var handler = new UpdateSaleHandler(saleRepo.Object, productRepo.Object);

            var request = new SaleUpdateRequest
            {
                Id = 1,
                CustomerId = 1,
                Items = new List<SaleItemUpdateRequest>
                {
                    new SaleItemUpdateRequest { ProductId = 1, Price = 10m, Quantity = 1 }
                }
            };

            // act
            Func<Task> act = () => handler.ExecuteAsync(request);

            // assert
            await act.Should()
                     .ThrowAsync<Exception>()
                     .WithMessage("product not found");
            saleRepo.Verify(r => r.UpdateAsync(It.IsAny<SaleEntity>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Throw_When_Insufficient_Stock()
        {
            // arrange
            var saleRepo = new Mock<ISaleRepository>();
            var productRepo = new Mock<IProductRepository>();

            var existingSale = new SaleEntity
            {
                Id = 1,
                CustomerId = 1,
                CreatedAt = DateTime.UtcNow,
                TotalAmount = 0m,
                Items = new List<RO.DevTest.Domain.Entities.SaleItem>()
            };
            saleRepo.Setup(r => r.GetByIdAsync(1))
                    .ReturnsAsync(existingSale);

            var product = new ProductEntity { Id = 1, Code = "P001", Price = 10m, StockQuantity = 0 };
            productRepo.Setup(r => r.GetByIdAsync(1))
                       .ReturnsAsync(product);

            var handler = new UpdateSaleHandler(saleRepo.Object, productRepo.Object);

            var request = new SaleUpdateRequest
            {
                Id = 1,
                CustomerId = 1,
                Items = new List<SaleItemUpdateRequest>
                {
                    new SaleItemUpdateRequest { ProductId = 1, Price = 10m, Quantity = 1 }
                }
            };

            // act
            Func<Task> act = () => handler.ExecuteAsync(request);

            // assert
            await act.Should()
                     .ThrowAsync<Exception>()
                     .WithMessage("insufficient stock quantity");
            saleRepo.Verify(r => r.UpdateAsync(It.IsAny<SaleEntity>()), Times.Never);
        }
    }
}
