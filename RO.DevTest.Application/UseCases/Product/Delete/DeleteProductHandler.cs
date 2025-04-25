using RO.DevTest.Application.Interfaces.Repositories;
using RO.DevTest.Application.Interfaces.UseCases.Product;

namespace RO.DevTest.Application.UseCases.Product.Delete
{
    // handles the deletion of a product
    public class DeleteProductHandler : IDeleteProductHandler
    {
        private readonly IProductRepository _repository;

        // inject repository to access data
        public DeleteProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> HandleAsync(int id)
        {
            // try to delete product; repository returns if existed
            return await _repository.DeleteAsync(id);
        }
    }

}
