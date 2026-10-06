using CafeManagement.API.Application.Services;
using CafeManagement.API.Domain.Entities;
using CafeManagement.Shared.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CafeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly IMCustomerService _customerService;
        private readonly IMOrderService _orderService;

        public SyncController(IMCustomerService customerService, IMOrderService orderService)
        {
            _customerService = customerService;
            _orderService = orderService;
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

        [HttpPost("order")]
        public async Task<IActionResult> SyncOrderPlaced(OrderPlacedEvent e)
        {
            MOrder? existing = await _orderService.GetOrderBySourceIdAsync(e.OrderId);

            if (existing != null)
            {
                return Ok();
            }

            //MOrder order = await _orderService.CreateOrderAsync(e.OrderId, e.TableId, e.CustomerId);
            //foreach (var item in e.Items)
            //{
            //    await _orderService.AddItemToOrderAsync(
            //        order,
            //        item.MenuItemId,
            //        item.Name,
            //        item.Price,
            //        item.Quantity
            //    );
            //}

            //await _orderService.UpdateOrderAsync(order);

            var order = await _orderService.CreateOrderWithItemsAsync(e.OrderId, e.TableId, e.CustomerId, e.Items);

            return Ok();
        }
    }
}
