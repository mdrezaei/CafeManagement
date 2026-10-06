using MenuAndOrdering.API.Application.DTOs;
using MenuAndOrdering.API.Domain.Entities;
using MenuAndOrdering.API.Domain.ValueObjects;

namespace MenuAndOrdering.API.Application.Services
{
    public interface IOrderingService
    {
        Task<Order> AddOrderAsync(Guid customerId, int tableId, List<OrderItemRequest> items, string? customerNote = null);

        Task<IReadOnlyList<Order>> GetCustomerOrdersByDateAsync(Guid customerId, DateTime dateTime);
        Task<IReadOnlyList<Order>> GetAllCustomerOrdersAsync(Guid customerId);

        Task<Order?> GetOrderByIdAsync(Guid orderId);

        Task<Order> UpdateOrderStatusAsync(Order order, OrderStatus newStatus);

    }
}
