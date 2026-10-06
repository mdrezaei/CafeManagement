using MenuAndOrdering.API.Application.DTOs;
using MenuAndOrdering.API.Application.Services;
using MenuAndOrdering.API.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MenuAndOrdering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly ITableService _tableService;
        public TableController(ITableService tableService)
        {
            _tableService = tableService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TableDto>> GetTable(int id)
        {
            Table? table = await _tableService.GetTableByIdAsync(id);

            if (table == null) 
            {
                return NotFound();
            }

            TableDto result = new TableDto()
            {
                Id = table.Id,
                TableNumber = table.TableNumber
            };

            return Ok(result);

        }

    }
}
