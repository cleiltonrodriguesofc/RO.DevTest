using Microsoft.AspNetCore.Mvc;
using RO.DevTest.Application.DTOs.Customer;
using RO.DevTest.Application.Interfaces.UseCases.Customer;
using RO.DevTest.Application.Interfaces.Repositories; 

namespace RO.DevTest.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICreateCustomerHandler _createHandler;

    private readonly ICustomerRepository      _repository;  

    private readonly IUpdateCustomerHandler _updateHandler;

    private readonly IDeleteCustomerHandler _deleteHandler;
    

    public CustomerController(
        // create customer
        ICreateCustomerHandler createHandler,
        // update customer
        IUpdateCustomerHandler updateHandler,
        // delete customer
        IDeleteCustomerHandler deleteHandler,

        ICustomerRepository    repository)
    {
        _createHandler = createHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
        _repository    = repository;
    }

    // POST api/customer
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CustomerCreateRequest request)
    {
        var id = await _createHandler.ExecuteAsync(request);
        return CreatedAtAction(nameof(GetAll), new { id }, null);
    }

    // GET api/customer all customers
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _repository.GetAllAsync();
        return Ok(list);
    }
    // GET api/customer/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await _repository.GetByIdAsync(id);

        if (customer == null)
            return NotFound();

        return Ok(customer);
    }
    
    // PUT api/customer/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CustomerUpdateRequest request)
    {
        if (id != request.Id)
            return BadRequest("ID in URL and payload do not match.");

        var updated = await _updateHandler.HandleAsync(request);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    // DELETE api/customer/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        // attempt to delete customer
        var deleted = await _deleteHandler.HandleAsync(id);
        if (!deleted)
            return NotFound();

        // no content on successful delete
        return NoContent();
    }



}
