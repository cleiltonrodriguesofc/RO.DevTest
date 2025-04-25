using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

// DTO
using RO.DevTest.Application.DTOs.Product;
// Handler
using RO.DevTest.Application.UseCases.Product.Update;
// Interface de repositório
using RO.DevTest.Application.Interfaces.Repositories;
// Alias para entidade
using ProductEntity = RO.DevTest.Domain.Entities.Product;

namespace RO.DevTest.Tests.Unit.Application.UseCases.Product.Commands
{
    public class UpdateProductHandlerTests
    {
        [Fact]
        public async Task HandleAsync_Should_Update_Product_When_Product_Exists()
        {
            // arrange
            var repo = new Mock<IProductRepository>();

            var existingProduct = new ProductEntity
            {
                Id = 1,
                Code = "P001",
                Name = "Old Name",
                Description = "Old Desc",
                Price = 100,
                StockQuantity = 10
            };

            repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(existingProduct);

            repo.Setup(r => r.UpdateAsync(It.IsAny<ProductEntity>()))
                .Returns(Task.CompletedTask);

            var handler = new UpdateProductHandler(repo.Object);

            var request = new ProductUpdateRequest
            {
                Id = 1,
                Code = "P002",
                Name = "New Name",
                Description = "New Desc",
                Price = 200,
                StockQuantity = 20
            };

            // act
            var result = await handler.HandleAsync(request);

            // assert
            result.Should().BeTrue();
            existingProduct.Code.Should().Be("P002");
            existingProduct.Name.Should().Be("New Name");
            repo.Verify(r => r.UpdateAsync(It.IsAny<ProductEntity>()), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_False_When_Product_Does_Not_Exist()
        {
            // arrange
            var repo = new Mock<IProductRepository>();

            repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((ProductEntity)null!);

            var handler = new UpdateProductHandler(repo.Object);

            var request = new ProductUpdateRequest
            {
                Id = 99, // não existe
                Code = "P999",
                Name = "Ghost Product"
            };

            // act
            var result = await handler.HandleAsync(request);

            // assert
            result.Should().BeFalse();
            repo.Verify(r => r.UpdateAsync(It.IsAny<ProductEntity>()), Times.Never);
        }
    }
}
