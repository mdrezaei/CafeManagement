using CafeManagement.API.Application.DTOs;
using CafeManagement.API.Application.Services;
using CafeManagement.API.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CafeManagement.API.Controllers
{
    [Authorize]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class MTableController : ControllerBase
    {
        private readonly IMTableService _mTableService;
        private readonly QrCodeService _qrCodeService;
        public MTableController(IMTableService tableService, QrCodeService qrCodeService)
        {
            _mTableService = tableService;
            _qrCodeService = qrCodeService;
        }


        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<MTableDto>>> GetAllTables([FromQuery] bool includeDeleted = false)
        {
            IReadOnlyList<MTable> tables = await _mTableService.GetAllTablesAsync(includeDeleted);
            IReadOnlyList<MTableDto> result = tables.Select(t => new MTableDto()
            {
                Id = t.Id,
                TableNumber = t.TableNumber
            }).ToList();

            return Ok(result);

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MTableDto>> GetTableById(int id, [FromQuery] bool includeDeleted = false)
        {
            MTable? table = await _mTableService.GetTableByIdAsync(id, includeDeleted);

            if (table == null)
            {
                return NotFound();
            }

            MTableDto result = new MTableDto()
            {
                Id = table.Id,
                TableNumber = table.TableNumber
            };

            return Ok(result);

        }

        [HttpGet("by-name/{tableNumber}")]
        public async Task<ActionResult<MTableDto>> GetTableByNumber(string tableNumber, [FromQuery] bool includeDeleted = false)
        {
            MTable? table = await _mTableService.GetTableByTableNumberAsync(tableNumber, includeDeleted);

            if (table == null)
            {
                return NotFound();
            }

            MTableDto result = new MTableDto()
            {
                Id = table.Id,
                TableNumber = table.TableNumber
            };

            return Ok(result);

        }

        [HttpPost]
        public async Task<ActionResult> CreateTable([FromBody] CreateTableRequest createTableRequest)
        {
            MTable table = await _mTableService.CreateTableAsync(createTableRequest.TableNumber);

            MTableDto result = new MTableDto()
            {
                Id = table.Id,
                TableNumber = table.TableNumber
            };

            return CreatedAtAction(nameof(GetTableById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateTable(int id, [FromBody] UpdateTableRequest updateTableRequest)
        {
            MTable? table = await _mTableService.GetTableByIdAsync(id);

            if (table == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(updateTableRequest.TableNumber))
            {
                table = await _mTableService.UpdateTableNumberAsync(table, updateTableRequest.TableNumber);
            }

            MTableDto result = new MTableDto()
            {
                Id = table.Id,
                TableNumber = table.TableNumber
            };

            return Ok(result);

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> SoftDeleteTable(int id)
        {
            MTable? table = await _mTableService.GetTableByIdAsync(id, false);

            if(table == null)
            {
                return NotFound();
            }

            await _mTableService.SoftDeleteTableAsync(table);

            return NoContent();

        }

        [HttpGet("{id:int}/qrcode")]
        public async Task<ActionResult<string>> GetQrCode(int id)
        {
            MTable? table = await _mTableService.GetTableByIdAsync(id);
            if (table == null)
            {
                return NotFound();
            }
            string url = _qrCodeService.BuildQrUrl(table.Id);
            byte[] qrImage = _qrCodeService.GenerateQrCode(url);
            return File(qrImage, "image/png");
        }

    }
}
