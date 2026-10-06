using CafeManagement.API.Domain.Entities;
using CafeManagement.API.Domain.Interfaces;
using CafeManagement.API.Domain.ValueObjects;
using CafeManagement.Shared.Events;

namespace CafeManagement.API.Application.Services
{
    public class MOrderService : IMOrderService
    {
        private readonly IMOrderRepository _mOrderRepository;
        private readonly IOutboxService _outboxService;

        public MOrderService(IMOrderRepository mOrderRepository, IOutboxService outboxService)
        {
            _mOrderRepository = mOrderRepository;
            _outboxService = outboxService;
        }

        public async Task<MOrder> CreateOrderAsync(Guid sourceOrderId, int tableId, Guid customerId)
        {
            MOrder order = new MOrder(sourceOrderId, tableId, customerId);
            await _mOrderRepository.AddOrderAsync(order);
            return order;
        }

        public async Task<MOrder> CreateOrderWithItemsAsync(Guid sourceOrderId, int tableId, Guid customerId, List<OrderPlacedItem> items)
        {
            var order = new MOrder(sourceOrderId, tableId, customerId);

            foreach (var item in items)
            {
                order.AddItem(item.MenuItemId, item.Name, item.Price, item.Quantity);
            }

            await _mOrderRepository.AddOrderAsync(order); // فقط یک SaveChanges
            return order;
        }

        public async Task<MOrder> UpdateOrderAsync(MOrder order)
        {
            await _mOrderRepository.UpdateOrderAsync(order);
            return order;
        }

        public async Task<MOrder> AssignWaiterAsync(MOrder order, Guid id)
        {
            order.AssignWaiter(id);
            await _mOrderRepository.UpdateOrderAsync(order);
            return order;
        }

        public async Task<MOrder> AssignBaristaAsync(MOrder order, Guid id)
        {
            order.AssignBarista(id);
            await _mOrderRepository.UpdateOrderAsync(order);
            return order;
        }

        public async Task<MOrder> AssignChefAsync(MOrder order, Guid id)
        {
            order.AssignChef(id);
            await _mOrderRepository.UpdateOrderAsync(order);
            return order;
        }

        public async Task<MOrder> AssignCashierAsync(MOrder order, Guid id)
        {
            order.AssignCashier(id);
            await _mOrderRepository.UpdateOrderAsync(order);
            return order;
        }

        public async Task<MOrder> AddItemToOrderAsync(MOrder order, Guid menuItemId, string orderedItemName, decimal orderedUnitPrice, int quantity = 1)
        {
            //MOrder? orderToUpdate = await _mOrderRepository.GetOrderByIdAsync(order.Id);


            //orderToUpdate.AddItem(menuItemId, orderedItemName, orderedUnitPrice, quantity);
            ////await _mOrderRepository.UpdateOrderAsync(orderToUpdate);
            //return orderToUpdate;

            order.AddItem(menuItemId, orderedItemName, orderedUnitPrice, quantity);
            //await _mOrderRepository.UpdateOrderAsync(order);
            return order;
        }

        public async Task<MOrder> RemoveItemFromOrderAsync(MOrder order, Guid menuItemId, int quantity = 1)
        {
            order.RemoveItem(menuItemId, quantity);
            await _mOrderRepository.UpdateOrderAsync(order);
            return order;
        }

        public async Task<MOrder> ClearEmptyItemsAsync(MOrder order)
        {
            order.ClearEmptyItems();
            await _mOrderRepository.UpdateOrderAsync(order);
            return order;
        }

        public async Task<MOrder> UpdateOrderStatusAsync(MOrder order, MOrderStatus newStatus)
        {
            order.UpdateStatus(newStatus);
            await _mOrderRepository.UpdateOrderAsync(order);

            await _outboxService.AddMessageAsync(new OrderStatusChangedEvent
            {
                OrderId = order.SourceOrderId,
                Status = order.Status.ToString(),
                UpdatedAt = DateTime.UtcNow
            });

            return order;
        }

        public async Task<MOrder> ChangeTableAsync(MOrder order, int tableId)
        {
            order.ChangeTable(tableId);
            await _mOrderRepository.UpdateOrderAsync(order);
            return order;
        }

        public async Task<MOrder> SetCustomerNoteAsync(MOrder order, string note)
        {
            order.SetCustomerNote(note);
            await _mOrderRepository.UpdateOrderAsync(order);
            return order;
        }

        public async Task<MOrder> UpdateSourceOrderAsync(MOrder order, Guid id)
        {
            order.UpdateSourceOrder(id);
            await _mOrderRepository.UpdateOrderAsync(order);
            return order;
        }

        public async Task<MOrder> UpdateCustomerAsync(MOrder order, Guid id)
        {
            order.UpdateCustomer(id);
            await _mOrderRepository.UpdateOrderAsync(order);
            return order;
        }

        public async Task<MOrder> UpdateCreatedDateAsync(MOrder order, DateTime createdDate)
        {
            order.UpdateCreatedDate(createdDate);
            await _mOrderRepository.UpdateOrderAsync(order);
            return order;
        }

        public async Task SoftDeleteOrderAsync(MOrder order)
        {
            order.SoftDelete();
            await _mOrderRepository.UpdateOrderAsync(order);
        }

        public async Task<IReadOnlyList<MOrder>> GetAllOrdersAsync(bool includeDeleted = false)
        {
            return await _mOrderRepository.GetAllOrdersAsync(includeDeleted);
        }

        public async Task<MOrder?> GetOrderBySourceIdAsync(Guid id, bool includeDeleted = false)
        {
            return await _mOrderRepository.GetOrderBySourceIdAsync(id, includeDeleted);
        }
        public async Task<MOrder?> GetOrderByIdAsync(Guid id, bool includeDeleted = false)
        {
            return await _mOrderRepository.GetOrderByIdAsync(id, includeDeleted);
        }

        public async Task<IReadOnlyList<MOrder>> GetOrdersByStatusAsync(MOrderStatus orderStatus, DateTime? dateTime = null, bool includeDeleted = false)
        {
            return await _mOrderRepository.GetOrdersByStatusAsync(orderStatus, dateTime, includeDeleted);
        }

        public async Task<IReadOnlyList<MOrder>> GetCustomerOrdersByDateAsync(Guid customerId, DateTime dateTime, bool includeDeleted = false)
        {
            return await _mOrderRepository.GetCustomerOrdersByDateAsync(customerId, dateTime, includeDeleted);
        }

        public async Task<IReadOnlyList<MOrder>> GetAllCustomerOrdersAsync(Guid customerId, bool includeDeleted = false)
        {
            return await _mOrderRepository.GetAllCustomerOrdersAsync(customerId, includeDeleted);
        }

        public async Task<IReadOnlyList<MOrder>> GetTableOrdersForTodayAsync(int tableId, bool includeDeleted = false)
        {
            return await _mOrderRepository.GetTableOrdersForTodayAsync(tableId, includeDeleted);
        }

        public async Task<IReadOnlyList<MOrder>> GetOrdersByDateAsync(DateTime from, DateTime? to = null, bool includeDeleted = false)
        {
            return await _mOrderRepository.GetOrdersByDateAsync(from, to, includeDeleted);
        }

        public async Task<IReadOnlyList<MOrder>> GetOrdersByAssignedEmployeeAsync(Guid employeeId, bool includeDeleted = false)
        {
            return await _mOrderRepository.GetOrdersByAssignedEmployeeAsync(employeeId, includeDeleted);
        }
    }
}
