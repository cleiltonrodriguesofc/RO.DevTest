using Microsoft.AspNetCore.Mvc;
using RO.DevTest.Application.DTOs.Product;
using RO.DevTest.Application.Interfaces.UseCases.Product;
using RO.DevTest.Application.Interfaces.Repositories;

namespace RO.DevTest.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ICreateProductHandler _createHandler;
        private readonly IUpdateProductHandler _updateHandler;
        // private readonly IDeleteProductHandler _deleteHandler;
        private readonly IProductRepository _repository;

        public ProductController(
            ICreateProductHandler createHandler,
            IUpdateProductHandler updateHandler,
            // IDeleteProductHandler deleteHandler,
            IProductRepository repository)
        {
            _createHandler = createHandler;
            _updateHandler = updateHandler;
            // _deleteHandler = deleteHandler;
            _repository = repository;
        }

        // POST api/product
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateRequest request)
        {
            var id = await _createHandler.ExecuteAsync(request);
            return CreatedAtAction(nameof(GetAll), new { id }, null);
        }

        // GET api/product
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _repository.GetAllAsync();
            return Ok(list);
        }

        // // GET api/product/{id}
        // [HttpGet("{id}")]
        // public async Task<IActionResult> GetById(int id)
        // {
        //     var product = await _repository.GetByIdAsync(id);
        //     if (product == null)
        //         return NotFound();

        //     return Ok(product);
        // }

        // PUT api/product/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateRequest request)
        {
            if (id != request.Id)
                return BadRequest("ID in URL and payload do not match.");

            var updated = await _updateHandler.HandleAsync(request);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        // // DELETE api/product/{id}
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> Delete(int id)
        // {
        //     var deleted = await _deleteHandler.HandleAsync(id);
        //     if (!deleted)
        //         return NotFound();

        //     return NoContent();
        // }
    }
}
