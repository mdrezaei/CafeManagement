using MenuAndOrdering.API.Application.DTOs;
using MenuAndOrdering.API.Application.Services;
using MenuAndOrdering.API.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MenuAndOrdering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderingService _orderingService;
        private readonly ICustomerService _customerService;

        public OrderController(IOrderingService orderingService, ICustomerService customerService)
        {
            _orderingService = orderingService;
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> PlaceOrder([FromBody] PlaceOrderRequest request)
        {
            Order order = await _orderingService.AddOrderAsync(
                                                request.CustomerId,
                                                request.TableId,
                                                request.Items,
                                                request.CustomerNote
                                                );
            OrderDto result = MapToDto(order);

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, result);

        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderDto>> GetById(Guid id)
        {
            Order? order = await _orderingService.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(MapToDto(order));
        }

        [HttpGet("customer/{customerId:guid}")]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetAllCustomerOrders(Guid customerId)
        {
            IReadOnlyList<Order> orders = await _orderingService.GetAllCustomerOrdersAsync(customerId);
            IReadOnlyList<OrderDto> result = orders.Select(o => MapToDto(o)).ToList();

            return Ok(result);
        }

        [HttpGet("track")]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> TrackTodayOrders([FromQuery] string phoneNumber)
        {
            var customer = await _customerService.GetCustomerByPhoneNumberAsync(phoneNumber);
            if (customer == null)
                return NotFound("مشتری با این شماره تلفن یافت نشد.");

            var todayOrders = await _orderingService.GetCustomerOrdersByDateAsync(
                customer.Id,
                DateTime.UtcNow.Date
            );

            if (todayOrders.Count == 0)
                return NotFound("سفارشی برای امروز یافت نشد.");

            var result = todayOrders.Select(o => MapToDto(o)).ToList();
            return Ok(result);
        }


        private OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                Status = order.Status.ToString(),
                TotalPrice = order.TotalPrice,
                CustomerNote = order.CustomerNote,
                CustomerId = order.CustomerId,
                TableId = order.TableId,
                CreatedDate = order.CreatedDate,
                OrderItems = order.OrderItems.Select(item => new OrderItemDto
                {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    MenuItemId = item.MenuItemId,
                    OrderedItemName = item.OrderedItemName,
                    OrderedUnitPrice = item.OrderedUnitPrice
                }).ToList()
            };
        }
    }
}
