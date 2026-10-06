using MenuAndOrdering.API.Domain.Entities;
using MenuAndOrdering.API.Domain.ValueObjects;

namespace MenuAndOrdering.API.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);

        Task UpdateOrderAsync(Order order);

        Task<IReadOnlyList<Order>> GetAllOrdersAsync();

        Task<Order?> GetOrderByIdAsync(Guid id);

        Task<IReadOnlyList<Order>> GetOrdersByStatusAsync(OrderStatus orderStatus);

        Task<IReadOnlyList<Order>> GetCustomerOrdersByDateAsync(Guid customerId, DateTime dateTime);
        Task<IReadOnlyList<Order>> GetAllCustomerOrdersAsync(Guid customerId);

        Task<IReadOnlyList<Order>> GetTableOrdersForTodayAsync(int tableId);

        Task<IReadOnlyList<Order>> GetOrdersByDateAsync(DateTime from, DateTime? to = null);






    }
}
