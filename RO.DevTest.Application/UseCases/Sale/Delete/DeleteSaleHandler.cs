using RO.DevTest.Application.Interfaces.Repositories;
using RO.DevTest.Application.Interfaces.UseCases.Sale;

namespace RO.DevTest.Application.UseCases.Sale.Delete;

// handles the deletion of a sale
public class DeleteSaleHandler : IDeleteSaleHandler
{
    private readonly ISaleRepository _repository;

    // inject repository to access data
    public DeleteSaleHandler(ISaleRepository repository)
    {
        _repository = repository;
    }

    // delete a sale by id; returns true if deleted
    public async Task<bool> HandleAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}
