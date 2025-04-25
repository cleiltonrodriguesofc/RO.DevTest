using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

// DTO
using RO.DevTest.Application.DTOs.Product;
// Handler
using RO.DevTest.Application.UseCases.Product.Create;
// Interface de repositório
using RO.DevTest.Application.Interfaces.Repositories;
// Alias para entidade
using ProductEntity = RO.DevTest.Domain.Entities.Product;

namespace RO.DevTest.Tests.Unit.Application.UseCases.Product.Create
{
    public class CreateProductHandlerTests
    {
        [Fact]
        public async Task ExecuteAsync_Should_Create_Product_When_Code_Is_Unique()
        {
            // arrange
            var repo = new Mock<IProductRepository>();

            // código não existe ainda
            repo.Setup(r => r.GetByCodeAsync(It.IsAny<string>()))
                .ReturnsAsync((ProductEntity)null!);

            // mocka o AddAsync e seta o Id
            repo.Setup(r => r.AddAsync(It.IsAny<ProductEntity>()))
                .Callback<ProductEntity>(p => p.Id = 456)
                .Returns(Task.CompletedTask);

            var handler = new CreateProductHandler(repo.Object);

            var request = new ProductCreateRequest
            {
                Code = "P001",
                Name = "Produto Teste",
                Description = "Descrição teste",
                Price = 200,
                StockQuantity = 50
            };

            // act
            var id = await handler.ExecuteAsync(request);

            // assert
            id.Should().Be(456);
            repo.Verify(r => r.AddAsync(It.IsAny<ProductEntity>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Throw_When_Code_Already_Exists()
        {
            // arrange
            var repo = new Mock<IProductRepository>();

            repo.Setup(r => r.GetByCodeAsync(It.IsAny<string>()))
                .ReturnsAsync(new ProductEntity { Id = 1, Code = "P001" });

            var handler = new CreateProductHandler(repo.Object);

            var request = new ProductCreateRequest
            {
                Code = "P001",
                Name = "Produto Duplicado"
            };

            // act
            Func<Task> act = () => handler.ExecuteAsync(request);

            // assert
            await act.Should()
                    .ThrowAsync<Exception>()
                    .WithMessage("product code already exists");
        }
    }
}
