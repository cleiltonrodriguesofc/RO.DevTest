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
        private readonly ISaleRepository _repository;

        public SaleController(ICreateSaleHandler createHandler, ISaleRepository repository)
        {
            _createHandler = createHandler;
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
                return NotFound();

            return Ok(sale);
        }

        // GET api/sale
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sales = await _repository.GetAllAsync();
            return Ok(sales);
        }
    }
}
