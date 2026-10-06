using MenuAndOrdering.API.Domain.Entities;
using MenuAndOrdering.API.Domain.Interfaces;
using MenuAndOrdering.API.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace MenuAndOrdering.API.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MenuAndOrderingDbContext _context;

        public OrderRepository(MenuAndOrderingDbContext context)
        {
            _context = context;
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.Include(o => o.OrderItems).Where(o => !o.IsDeleted).ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(Guid id)
        {
            return await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersByStatusAsync(OrderStatus orderStatus)
        {
            return await _context.Orders.Include(o => o.OrderItems).Where(
                o => o.Status == orderStatus && !o.IsDeleted && o.CreatedDate.Date == DateTime.UtcNow.Date)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Order>> GetCustomerOrdersByDateAsync(Guid customerId, DateTime dateTime)
        {
            return await _context.Orders.Include(o => o.OrderItems).Where(
                o => o.CustomerId == customerId && o.CreatedDate.Date >= dateTime.Date)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Order>> GetAllCustomerOrdersAsync(Guid customerId)
        {
            return await _context.Orders.Include(o => o.OrderItems).Where(o => !o.IsDeleted && o.CustomerId == customerId).ToListAsync();
        }

        public async Task<IReadOnlyList<Order>> GetTableOrdersForTodayAsync(int tableId)
        {
            DateTime todayDate = DateTime.UtcNow;

            return await _context.Orders.Include(o => o.OrderItems).Where(
                o => o.TableId == tableId && o.CreatedDate.Date == todayDate.Date)
                .ToListAsync();

        }

        public async Task<IReadOnlyList<Order>> GetOrdersByDateAsync(DateTime from, DateTime? to = null)
        {
            DateTime endDate = to ?? DateTime.UtcNow;

            return await _context.Orders.Include(o => o.OrderItems).Where(o => o.CreatedDate >= from && o.CreatedDate <= endDate).ToListAsync();

        }
    }
}
