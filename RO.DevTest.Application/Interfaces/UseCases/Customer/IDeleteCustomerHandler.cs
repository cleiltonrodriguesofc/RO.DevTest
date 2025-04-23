namespace RO.DevTest.Application.Interfaces.UseCases.Customer;

// handles deleting a customer by id
public interface IDeleteCustomerHandler
{
    // returns true if deleted, false if not found
    Task<bool> HandleAsync(int id);
}
