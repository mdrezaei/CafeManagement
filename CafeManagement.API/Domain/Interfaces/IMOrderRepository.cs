using CafeManagement.API.Domain.Entities;
using CafeManagement.API.Domain.ValueObjects;

namespace CafeManagement.API.Domain.Interfaces
{
    public interface IMOrderRepository
    {
        Task AddOrderAsync(MOrder order);

        Task UpdateOrderAsync(MOrder order);

        Task<IReadOnlyList<MOrder>> GetAllOrdersAsync(bool includeDeleted = false);

        Task<MOrder?> GetOrderBySourceIdAsync(Guid id, bool includeDeleted = false);
        Task<MOrder?> GetOrderByIdAsync(Guid id, bool includeDeleted = false);

        Task<IReadOnlyList<MOrder>> GetOrdersByStatusAsync(MOrderStatus orderStatus, DateTime? dateTime = null, bool includeDeleted = false);

        Task<IReadOnlyList<MOrder>> GetCustomerOrdersByDateAsync(Guid customerId, DateTime dateTime, bool includeDeleted = false);
        Task<IReadOnlyList<MOrder>> GetAllCustomerOrdersAsync(Guid customerId, bool includeDeleted = false);

        Task<IReadOnlyList<MOrder>> GetTableOrdersForTodayAsync(int tableId, bool includeDeleted = false);

        Task<IReadOnlyList<MOrder>> GetOrdersByDateAsync(DateTime from, DateTime? to = null, bool includeDeleted = false);

        Task<IReadOnlyList<MOrder>> GetOrdersByAssignedEmployeeAsync(Guid employeeId, bool includeDeleted = false);

    }
}
