using CafeManagement.Shared.Events;
using MenuAndOrdering.API.Application.DTOs;
using MenuAndOrdering.API.Domain.Entities;
using MenuAndOrdering.API.Domain.Interfaces;
using MenuAndOrdering.API.Domain.ValueObjects;
using MenuAndOrdering.API.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

namespace MenuAndOrdering.API.Application.Services
{
    public class OrderingService : IOrderingService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IOutboxService _outboxService;

        public OrderingService(IOrderRepository orderRepository, IMenuItemRepository menuItemRepository, IOutboxService outboxService)
        {
            _orderRepository = orderRepository;
            _menuItemRepository = menuItemRepository;
            _outboxService = outboxService;
        }

        public async Task<Order> AddOrderAsync(Guid customerId, int tableId, List<OrderItemRequest> items, string? customerNote = null)
        {
            Order order = new Order(customerId, tableId, customerNote);

            foreach (OrderItemRequest item in items)
            {
                MenuItem? menuItem = await _menuItemRepository.GetMenuItemByIdAsync(item.MenuItemId);
                if (menuItem != null)
                {
                    order.AddItem(menuItem.Id, menuItem.Name, menuItem.Price, item.Quantity);
                }
            }
            order.ClearEmptyItems();
            await _orderRepository.AddOrderAsync(order);

            await _outboxService.AddMessageAsync(new OrderPlacedEvent
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                TableId = order.TableId,
                CreatedAt = order.CreatedDate,
                Items = order.OrderItems.Select(oi => new OrderPlacedItem
                {
                    MenuItemId = oi.MenuItemId,
                    Name = oi.OrderedItemName,
                    Price = oi.OrderedUnitPrice,
                    Quantity = oi.Quantity
                }).ToList()
            });

            return order;

        }

        public async Task<Order> UpdateOrderStatusAsync(Order order, OrderStatus newStatus)
        {
            order.UpdateStatus(newStatus);
            await _orderRepository.UpdateOrderAsync(order);
            return order;
        }


        public async Task<IReadOnlyList<Order>> GetCustomerOrdersByDateAsync(Guid customerId, DateTime dateTime)
        {
            return await _orderRepository.GetCustomerOrdersByDateAsync(customerId, dateTime);
        }

        public async Task<IReadOnlyList<Order>> GetAllCustomerOrdersAsync(Guid customerId)
        {
            return await _orderRepository.GetAllCustomerOrdersAsync(customerId);
        }

        public async Task<Order?> GetOrderByIdAsync(Guid orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }




    }
}
