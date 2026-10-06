using CafeManagement.Shared.Events;
using MenuAndOrdering.API.Application.Services;
using MenuAndOrdering.API.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MenuAndOrdering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;
        private readonly ITableService _tableService;
        private readonly ICustomerService _customerService;
        private readonly IOrderingService _orderingService;

        public SyncController(IMenuItemService menuItemService, ITableService tableService, ICustomerService customerService, IOrderingService orderingService)
        {
            _menuItemService = menuItemService;
            _tableService = tableService;
            _customerService = customerService;
            _orderingService = orderingService;
        }

        [HttpPost("menuitem")]
        public async Task<IActionResult> SyncMenuItemCreated(MenuItemCreatedEvent e)
        {
            var existing = await _menuItemService.GetMenuItemByIdAsync(e.MenuItemId);
            if (existing != null)
            {
                return Ok();
            }

            await _menuItemService.CreateMenuItemAsync(e.Name, e.Category, e.Price, null, e.MenuItemId);
            return Ok();
        }

        [HttpPut("menuitem")]
        public async Task<IActionResult> SyncMenuItemUpdated(MenuItemUpdatedEvent e)
        {
            var menuItem = await _menuItemService.GetMenuItemByIdAsync(e.MenuItemId);
            if (menuItem == null)
            {
                return NotFound();
            }
            await _menuItemService.UpdateMenuItemNameAsync(menuItem, e.Name);
            await _menuItemService.UpdateMenuItemPriceAsync(menuItem, e.Price);
            await _menuItemService.UpdateMenuItemCategoryAsync(menuItem, e.Category);
            return Ok();
        }

        [HttpDelete("menuitem/delete")]
        public async Task<IActionResult> SyncMenuItemDeleted(MenuItemDeletedEvent e)
        {
            var menuItem = await _menuItemService.GetMenuItemByIdAsync(e.MenuItemId);
            if (menuItem == null)
            {
                return NotFound();
            }
            await _menuItemService.SoftDeleteMenuItemAsync(menuItem);
            return Ok();
        }

        [HttpPost("table")]
        public async Task<IActionResult> SyncTableCreated(TableCreatedEvent e)
        {
            var existing = await _tableService.GetTableByIdAsync(e.TableId);
            if (existing != null)
            {
                return Ok();
            }

            await _tableService.CreateTableAsync(e.TableNumber);
            return Ok();
        }

        [HttpPut("table")]
        public async Task<IActionResult> SyncTableUpdated(TableUpdatedEvent e)
        {
            var table = await _tableService.GetTableByIdAsync(e.TableId);
            if (table == null)
            {
                return NotFound();
            }
            await _tableService.UpdateTableNumberAsync(table, e.TableNumber);
            return Ok();
        }

        [HttpDelete("table/delete")]
        public async Task<IActionResult> SyncTableDeleted(TableDeletedEvent e)
        {
            var table = await _tableService.GetTableByIdAsync(e.TableId);
            if (table == null)
            {
                return NotFound();
            }
            await _tableService.SoftDeleteTableAsync(table);
            return Ok();
        }

        [HttpPost("customer")]
        public async Task<IActionResult> SyncCustomerCreated(CustomerCreatedEvent e)
        {
            var existing = await _customerService.GetCustomerByPhoneNumberAsync(e.PhoneNumber);
            if (existing != null)
            {
                return Ok();
            }
            await _customerService.CreateCustomerAsync(e.Name, e.PhoneNumber, e.CustomerId);
            return Ok();
        }

        [HttpPut("customer")]
        public async Task<IActionResult> SyncCustomerUpdated(CustomerUpdatedEvent e)
        {
            var customer = await _customerService.GetCustomerByIdAsync(e.CustomerId);
            if (customer == null)
            {
                return NotFound();
            }
            await _customerService.UpdateCustomerNameAsync(customer, e.Name);
            await _customerService.UpdateCustomerPhoneNumberAsync(customer, e.PhoneNumber);
            return Ok();
        }

        [HttpDelete("customer/delete")]
        public async Task<IActionResult> SyncCustomerDeleted(CustomerDeletedEvent e)
        {
            var customer = await _customerService.GetCustomerByIdAsync(e.CustomerId);
            if (customer == null)
            {
                return NotFound();
            }
            await _customerService.SoftDeleteCustomerAsync(customer);
            return Ok();
        }

        [HttpPatch("order/status")]
        public async Task<IActionResult> SyncOrderStatusChanged(OrderStatusChangedEvent e)
        {
            var order = await _orderingService.GetOrderByIdAsync(e.OrderId);
            if (order == null)
            {
                return NotFound();
            }
            if (Enum.TryParse<OrderStatus>(e.Status, true, out var newStatus))
            {
                await _orderingService.UpdateOrderStatusAsync(order, newStatus);
            }

            return Ok();
        }
    }
}
