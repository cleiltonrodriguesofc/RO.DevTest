using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

// dto for create request
using RO.DevTest.Application.DTOs.Customer;
// handler under test
using RO.DevTest.Application.UseCases.Customer.Create;
// repository contract
using RO.DevTest.Application.Interfaces.Repositories;
// alias for the domain entity
using CustomerEntity = RO.DevTest.Domain.Entities.Customer;

namespace RO.DevTest.Tests.Unit.Application.UseCases.Customer.Create
{
    public class CreateCustomerHandlerTests
    {
        [Fact]
        public async Task ExecuteAsync_Should_Create_Customer_Successfully()
        {
            // arrange: mock repository
            var repo = new Mock<ICustomerRepository>();

            // email does not exist
            repo.Setup(r => r.EmailExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            // when add is called, assign an id to the entity
            repo.Setup(r => r.AddAsync(It.IsAny<CustomerEntity>()))
                .Callback<CustomerEntity>(cust => cust.Id = 123)
                .Returns(Task.CompletedTask);

            var handler = new CreateCustomerHandler(repo.Object);

            var request = new CustomerCreateRequest
            {
                Name    = "John Doe",
                Email   = "john.doe@example.com",
                Address = "Rua 1"
            };

            // act
            var id = await handler.ExecuteAsync(request);

            // assert: id was set by the mock callback
            id.Should().Be(123);

            // verify add was called once
            repo.Verify(r => r.AddAsync(It.IsAny<CustomerEntity>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Throw_When_Email_Already_Exists()
        {
            // arrange: mock repository returning true for existing email
            var repo = new Mock<ICustomerRepository>();
            repo.Setup(r => r.EmailExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var handler = new CreateCustomerHandler(repo.Object);

            var request = new CustomerCreateRequest
            {
                Name    = "Jane Doe",
                Email   = "jane.doe@example.com",
                Address = "Rua 2"
            };

            // act
            Func<Task> act = () => handler.ExecuteAsync(request);

            // assert: exception thrown for duplicate email
            await act.Should()
                     .ThrowAsync<InvalidOperationException>()
                     .WithMessage("email already in use");
        }
    }
}
