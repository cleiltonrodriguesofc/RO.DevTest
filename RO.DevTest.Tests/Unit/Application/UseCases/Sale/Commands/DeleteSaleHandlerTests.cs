using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

// handler
using RO.DevTest.Application.UseCases.Sale.Delete;
// repository interface
using RO.DevTest.Application.Interfaces.Repositories;
// alias for entity
using SaleEntity = RO.DevTest.Domain.Entities.Sale;

namespace RO.DevTest.Tests.Unit.Application.UseCases.Sale.Commands
{
    public class DeleteSaleHandlerTests
    {
        [Fact]
        public async Task HandleAsync_Should_Delete_Sale_When_Sale_Exists()
        {
            // arrange
            var saleRepo = new Mock<ISaleRepository>();

            // setup repository to return true when deleting existing sale
            saleRepo.Setup(r => r.DeleteAsync(1))
                    .ReturnsAsync(true);

            var handler = new DeleteSaleHandler(saleRepo.Object);

            // act
            var result = await handler.HandleAsync(1);

            // assert
            result.Should().BeTrue();
            saleRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_False_When_Sale_Does_Not_Exist()
        {
            // arrange
            var saleRepo = new Mock<ISaleRepository>();

            // setup repository to return false when deleting non-existing sale
            saleRepo.Setup(r => r.DeleteAsync(99))
                    .ReturnsAsync(false);

            var handler = new DeleteSaleHandler(saleRepo.Object);

            // act
            var result = await handler.HandleAsync(99);

            // assert
            result.Should().BeFalse();
            saleRepo.Verify(r => r.DeleteAsync(99), Times.Once);
        }
    }
}
