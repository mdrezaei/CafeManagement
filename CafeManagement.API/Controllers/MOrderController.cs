using CafeManagement.API.Application.DTOs;
using CafeManagement.API.Application.Services;
using CafeManagement.API.Domain.Entities;
using CafeManagement.API.Domain.Interfaces;
using CafeManagement.API.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CafeManagement.API.Controllers
{
    [Authorize]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class MOrderController : ControllerBase
    {
        private readonly IMOrderService _mOrderService;
        private readonly IMMenuItemService _mMenuItemService;
        private readonly IMCustomerRepository _mCustomerRepository;

        public MOrderController(IMOrderService mOrderService, IMMenuItemService mMenuItemService, IMCustomerRepository mCustomerRepository)
        {
            _mOrderService = mOrderService;
            _mMenuItemService = mMenuItemService;
            _mCustomerRepository = mCustomerRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<MOrderDto>>> GetAllOrders( [FromQuery] bool includeDeleted = false)
        {
            IReadOnlyList<MOrder> orders = await _mOrderService.GetAllOrdersAsync(includeDeleted);

            var result = new List<MOrderDto>();

            foreach (var order in orders)
            {
                result.Add(await MapToDtoAsync(order));
            }
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MOrderDto>> GetOrderById(Guid id, [FromQuery] bool includeDeleted = false)
        {
            MOrder? order = await _mOrderService.GetOrderByIdAsync(id, includeDeleted);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(await MapToDtoAsync(order));
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IReadOnlyList<MOrderDto>>> GetOrdersByStatus(string status, [FromQuery] DateTime? dateTime = null, [FromQuery] bool includeDeleted = false)
        {
            if (!Enum.TryParse<MOrderStatus>(status, true, out var orderStatus))
            {
                return BadRequest("وضعیت نامعتبر است.");
            }
            IReadOnlyList<MOrder> orders = await _mOrderService.GetOrdersByStatusAsync(orderStatus, dateTime, includeDeleted);

            var result = new List<MOrderDto>();

            foreach (var order in orders)
            {
                result.Add(await MapToDtoAsync(order));
            }
            return Ok(result);
        }

        [HttpGet("customer/{customerId:guid}")]
        public async Task<ActionResult<IReadOnlyList<MOrderDto>>> GetCustomerOrders(Guid customerId, [FromQuery] DateTime? date = null, [FromQuery] bool includeDeleted = false)
        {
            IReadOnlyList<MOrder> orders;

            if (date.HasValue)
            {
                orders = await _mOrderService.GetCustomerOrdersByDateAsync(customerId, date.Value, includeDeleted);
            }
            else
            {
                orders = await _mOrderService.GetAllCustomerOrdersAsync(customerId, includeDeleted);
            }

            var result = new List<MOrderDto>();

            foreach (var order in orders)
            {
                result.Add(await MapToDtoAsync(order));
            }
            return Ok(result);
        }

        [HttpGet("table/{tableId:int}/today")]
        public async Task<ActionResult<IReadOnlyList<MOrderDto>>> GetTableOrdersForToday(int tableId, [FromQuery] bool includeDeleted = false)
        {
            IReadOnlyList<MOrder> orders = await _mOrderService.GetTableOrdersForTodayAsync(tableId, includeDeleted);

            var result = new List<MOrderDto>();

            foreach (var order in orders)
            {
                result.Add(await MapToDtoAsync(order));
            }
            return Ok(result);
        }

        [HttpGet("by-date")]
        public async Task<ActionResult<IReadOnlyList<MOrderDto>>> GetOrdersByDate([FromQuery] DateTime from, [FromQuery] DateTime? to = null, [FromQuery] bool includeDeleted = false)
        {
            IReadOnlyList<MOrder> orders = await _mOrderService.GetOrdersByDateAsync(from, to, includeDeleted);

            var result = new List<MOrderDto>();

            foreach (var order in orders)
            {
                result.Add(await MapToDtoAsync(order));
            }
            return Ok(result);
        }

        [HttpGet("employee/{employeeId:guid}/assigned")]
        public async Task<ActionResult<IReadOnlyList<MOrderDto>>> GetOrdersByAssignedEmployee(Guid employeeId, [FromQuery] bool includeDeleted = false)
        {
            IReadOnlyList<MOrder> orders = await _mOrderService.GetOrdersByAssignedEmployeeAsync(employeeId, includeDeleted);

            var result = new List<MOrderDto>();

            foreach (var order in orders)
            {
                result.Add(await MapToDtoAsync(order));
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<MOrderDto>> CreateOrder([FromBody] CreateOrderRequest request)
        {
            MOrder order = await _mOrderService.CreateOrderAsync(request.SourceOrderId, request.TableId, request.CustomerId);

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, await MapToDtoAsync(order));
        }

        [HttpPatch("{id:guid}/status")]
        public async Task<ActionResult<MOrderDto>> UpdateOrderStatus(Guid id, [FromBody] UpdateOrderStatusRequest request)
        {
            MOrder? order = await _mOrderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            if (!Enum.TryParse<MOrderStatus>(request.Status, true, out var newStatus))
            {
                return BadRequest("وضعیت نامعتبر است.");
            }
            order = await _mOrderService.UpdateOrderStatusAsync(order, newStatus);
            return Ok(await MapToDtoAsync(order));
        }

        [HttpPatch("{id:guid}/assign/waiter")]
        public async Task<ActionResult<MOrderDto>> AssignWaiter(Guid id, [FromBody] AssignEmployeeRequest request)
        {
            MOrder? order = await _mOrderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            order = await _mOrderService.AssignWaiterAsync(order, request.EmployeeId);
            return Ok(await MapToDtoAsync(order));
        }

        [HttpPatch("{id:guid}/assign/barista")]
        public async Task<ActionResult<MOrderDto>> AssignBarista(Guid id, [FromBody] AssignEmployeeRequest request)
        {
            MOrder? order = await _mOrderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            order = await _mOrderService.AssignBaristaAsync(order, request.EmployeeId);
            return Ok(await MapToDtoAsync(order));
        }

        [HttpPatch("{id:guid}/assign/chef")]
        public async Task<ActionResult<MOrderDto>> AssignChef(Guid id, [FromBody] AssignEmployeeRequest request)
        {
            MOrder? order = await _mOrderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            order = await _mOrderService.AssignChefAsync(order, request.EmployeeId);
            return Ok(await MapToDtoAsync(order));
        }

        [HttpPatch("{id:guid}/assign/cashier")]
        public async Task<ActionResult<MOrderDto>> AssignCashier(Guid id, [FromBody] AssignEmployeeRequest request)
        {
            MOrder? order = await _mOrderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            order = await _mOrderService.AssignCashierAsync(order, request.EmployeeId);
            return Ok(await MapToDtoAsync(order));
        }

        [HttpPost("{id:guid}/items")]
        public async Task<ActionResult<MOrderDto>> AddItemToOrder(Guid id, [FromBody] AddOrderItemRequest request)
        {
            MOrder? order = await _mOrderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            MMenuItem? menuItem = await _mMenuItemService.GetMenuItemByIdAsync(request.MenuItemId);
            if (menuItem == null)
            {
                return BadRequest("آیتم منو پیدا نشد.");
            }
            order = await _mOrderService.AddItemToOrderAsync(
                order,
                menuItem.Id,
                menuItem.Name,
                menuItem.Price,
                request.Quantity
            );

            return Ok(await MapToDtoAsync(order));
        }

        [HttpDelete("{id:guid}/items/{menuItemId:guid}")]
        public async Task<ActionResult<MOrderDto>> RemoveItemFromOrder(Guid id, Guid menuItemId, [FromQuery] int quantity = 1)
        {
            MOrder? order = await _mOrderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            order = await _mOrderService.RemoveItemFromOrderAsync(order, menuItemId, quantity);
            order = await _mOrderService.ClearEmptyItemsAsync(order);
            return Ok(await MapToDtoAsync(order));
        }

        [HttpPatch("{id:guid}/clear-empty-items")]
        public async Task<ActionResult<MOrderDto>> ClearEmptyItems(Guid id)
        {
            MOrder? order = await _mOrderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            order = await _mOrderService.ClearEmptyItemsAsync(order);
            return Ok(await MapToDtoAsync(order));
        }

        [HttpPatch("{id:guid}/table")]
        public async Task<ActionResult<MOrderDto>> ChangeTable(Guid id, [FromBody] ChangeTableRequest request)
        {
            MOrder? order = await _mOrderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            order = await _mOrderService.ChangeTableAsync(order, request.TableId);
            return Ok(await MapToDtoAsync(order));
        }

        [HttpPatch("{id:guid}/note")]
        public async Task<ActionResult<MOrderDto>> SetCustomerNote(Guid id, [FromBody] SetCustomerNoteRequest request)
        {
            MOrder? order = await _mOrderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            order = await _mOrderService.SetCustomerNoteAsync(order, request.Note ?? string.Empty);
            return Ok(await MapToDtoAsync(order));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> SoftDeleteOrder(Guid id)
        {
            MOrder? order = await _mOrderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            await _mOrderService.SoftDeleteOrderAsync(order);
            return NoContent();
        }

        private async Task<MOrderDto> MapToDtoAsync(MOrder order)
        {
            var dto = new MOrderDto
            {
                Id = order.Id,
                OrderItems = order.OrderItems.Select(item => new MOrderItemDto
                {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    MenuItemId = item.MenuItemId,
                    OrderedItemName = item.OrderedItemName,
                    OrderedUnitPrice = item.OrderedUnitPrice
                }).ToList(),
                Status = order.Status.ToString(),
                TotalPrice = order.TotalPrice,
                CustomerId = order.CustomerId,
                CustomerNote = order.CustomerNote,
                TableId = order.TableId,
                CreatedDate = order.CreatedDate,
                SourceOrderId = order.SourceOrderId,
                AssignedWaiterId = order.AssignedWaiterId,
                AssignedBaristaId = order.AssignedBaristaId,
                AssignedChefId = order.AssignedChefId,
                AssignedCashierId = order.AssignedCashierId
            };

            var customer = await _mCustomerRepository.GetCustomerByIdAsync(order.CustomerId);
            dto.CustomerName = customer?.Name;

            return dto;
        }
    }
}
