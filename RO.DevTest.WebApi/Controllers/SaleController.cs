using Microsoft.AspNetCore.Mvc;
using RO.DevTest.Application.DTOs.Sale;
using RO.DevTest.Application.Interfaces.UseCases.Sale;
using RO.DevTest.Application.Interfaces.Repositories;

namespace RO.DevTest.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly ICreateSaleHandler _createHandler;
        private readonly IUpdateSaleHandler _updateHandler;
        private readonly IDeleteSaleHandler _deleteHandler;  
        private readonly ISaleRepository _repository;

        public SaleController(
            ICreateSaleHandler createHandler,
            IUpdateSaleHandler updateHandler,
            IDeleteSaleHandler deleteHandler,  // inject delete handler
            ISaleRepository repository)
        {
            _createHandler = createHandler;
            _updateHandler = updateHandler;
            _deleteHandler = deleteHandler;
            _repository = repository;
        }

        // POST api/sale
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SaleCreateRequest request)
        {
            var saleId = await _createHandler.ExecuteAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = saleId }, null);
        }

        // GET api/sale/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sale = await _repository.GetByIdAsync(id);
            if (sale == null)
                return NotFound();  // return 404 if sale not found

            // project only necessary fields
            var result = new
            {
                saleId = sale.Id,
                customerName = sale.Customer.Name,  // include customer name
                items = sale.Items.Select(i => new
                {
                    productCode = i.Product.Code,   // include product code
                    productName = i.Product.Name,   // include product name
                    price = i.Price,                // include sale price
                    quantity = i.Quantity           // include quantity
                })
            };

            return Ok(result);
        }

        // GET api/sale
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sales = await _repository.GetAllAsync();

            // project only necessary fields for each sale
            var result = sales.Select(s => new
            {
                saleId = s.Id,
                customerName = s.Customer.Name,
                items = s.Items.Select(i => new
                {
                    productCode = i.Product.Code,
                    productName = i.Product.Name,
                    price = i.Price,
                    quantity = i.Quantity
                })
            });

            return Ok(result);
        }

        // PUT api/sale/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SaleUpdateRequest request)
        {
            var existingSale = await _repository.GetByIdAsync(id);
            if (existingSale == null)
                return NotFound();  // return 404 if sale not found

            await _updateHandler.ExecuteAsync(request);

            return NoContent();  // return 204 on successful update
        }

        // DELETE api/sale/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _deleteHandler.HandleAsync(id);
            if (!deleted)
                return NotFound();  // return 404 if sale not found or already deleted

            return NoContent();  // return 204 on successful deletion
        }
    }
}
