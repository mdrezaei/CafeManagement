using CafeManagement.API.Domain.Entities;
using CafeManagement.API.Domain.ValueObjects;
using CafeManagement.Shared.Events;

namespace CafeManagement.API.Application.Services
{
    public interface IMOrderService
    {
        Task<MOrder> CreateOrderWithItemsAsync(Guid sourceOrderId, int tableId, Guid customerId, List<OrderPlacedItem> items);
        Task<MOrder> CreateOrderAsync(Guid sourceOrderId, int tableId, Guid customerId);
        Task<MOrder> UpdateOrderAsync(MOrder order);
        Task<MOrder> AssignWaiterAsync(MOrder order, Guid id);
        Task<MOrder> AssignBaristaAsync(MOrder order, Guid id);
        Task<MOrder> AssignChefAsync(MOrder order, Guid id);
        Task<MOrder> AssignCashierAsync(MOrder order, Guid id);
        Task<MOrder> AddItemToOrderAsync(MOrder order, Guid menuItemId, string orderedItemName, decimal orderedUnitPrice, int quantity = 1);
        Task<MOrder> RemoveItemFromOrderAsync(MOrder order, Guid menuItemId, int quantity = 1);
        Task<MOrder> ClearEmptyItemsAsync(MOrder order);
        Task<MOrder> UpdateOrderStatusAsync(MOrder order, MOrderStatus newStatus);
        Task<MOrder> ChangeTableAsync(MOrder order, int tableId);
        Task<MOrder> SetCustomerNoteAsync(MOrder order, string note);
        Task<MOrder> UpdateSourceOrderAsync(MOrder order, Guid id);
        Task<MOrder> UpdateCustomerAsync(MOrder order, Guid id);
        Task<MOrder> UpdateCreatedDateAsync(MOrder order, DateTime createdDate);
        Task SoftDeleteOrderAsync(MOrder order);
        Task<IReadOnlyList<MOrder>> GetAllOrdersAsync(bool includeDeleted = false);
        Task<MOrder?> GetOrderByIdAsync(Guid id, bool includeDeleted = false);
        Task<MOrder?> GetOrderBySourceIdAsync(Guid id, bool includeDeleted = false);
        Task<IReadOnlyList<MOrder>> GetOrdersByStatusAsync(MOrderStatus orderStatus, DateTime? dateTime = null, bool includeDeleted = false);
        Task<IReadOnlyList<MOrder>> GetCustomerOrdersByDateAsync(Guid customerId, DateTime dateTime, bool includeDeleted = false);
        Task<IReadOnlyList<MOrder>> GetAllCustomerOrdersAsync(Guid customerId, bool includeDeleted = false);
        Task<IReadOnlyList<MOrder>> GetTableOrdersForTodayAsync(int tableId, bool includeDeleted = false);
        Task<IReadOnlyList<MOrder>> GetOrdersByDateAsync(DateTime from, DateTime? to = null, bool includeDeleted = false);
        Task<IReadOnlyList<MOrder>> GetOrdersByAssignedEmployeeAsync(Guid employeeId, bool includeDeleted = false);


    }
}
