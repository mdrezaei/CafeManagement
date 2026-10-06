using CafeManagement.API.Domain.Entities;
using CafeManagement.API.Domain.Interfaces;
using CafeManagement.API.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CafeManagement.API.Infrastructure.Persistence.Repositories
{
    public class MOrderRepository : IMOrderRepository
    {
        private readonly CafeManagementDbContext _context;
        public MOrderRepository(CafeManagementDbContext context)
        {
            _context = context;
        }

        public async Task AddOrderAsync(MOrder order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(MOrder order)
        {
            //_context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<MOrder>> GetAllOrdersAsync(bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return await _context.Orders.Include(o => o.OrderItems).ToListAsync();
            }
            else
            {
                return await _context.Orders.Include(o => o.OrderItems).Where(o => !o.IsDeleted).ToListAsync();
            }
        }

        public async Task<MOrder?> GetOrderBySourceIdAsync(Guid id, bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.SourceOrderId == id); ;
            }
            else
            {
                return await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.SourceOrderId == id && !o.IsDeleted);
            }
        }

        public async Task<MOrder?> GetOrderByIdAsync(Guid id, bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id); ;
            }
            else
            {
                return await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);
            }
        }

        public async Task<IReadOnlyList<MOrder>> GetOrdersByStatusAsync(MOrderStatus orderStatus, DateTime? dateTime = null, bool includeDeleted = false)
        {
            DateTime date = dateTime ?? DateTime.UtcNow;

            if (includeDeleted)
            {
                return await _context.Orders.Include(o => o.OrderItems).Where(o => o.Status == orderStatus && o.CreatedDate.Date == date.Date).ToListAsync();
            }
            else
            {
                return await _context.Orders.Include(o => o.OrderItems).Where(
                    o => o.Status == orderStatus && o.CreatedDate.Date == date.Date && !o.IsDeleted).ToListAsync();
            }

        }

        public async Task<IReadOnlyList<MOrder>> GetCustomerOrdersByDateAsync(Guid customerId, DateTime dateTime, bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return await _context.Orders.Include(o => o.OrderItems).Where(
                o => o.CustomerId == customerId && o.CreatedDate.Date >= dateTime.Date)
                .ToListAsync();
            }
            else
            {
                return await _context.Orders.Include(o => o.OrderItems).Where(
                o => o.CustomerId == customerId && o.CreatedDate.Date >= dateTime.Date && !o.IsDeleted)
                .ToListAsync();
            }

        }

        public async Task<IReadOnlyList<MOrder>> GetAllCustomerOrdersAsync(Guid customerId, bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return await _context.Orders.Include(o => o.OrderItems).Where(
                o => o.CustomerId == customerId)
                .ToListAsync();
            }
            else
            {
                return await _context.Orders.Include(o => o.OrderItems).Where(
                o => o.CustomerId == customerId && !o.IsDeleted)
                .ToListAsync();
            }
        }

        public async Task<IReadOnlyList<MOrder>> GetTableOrdersForTodayAsync(int tableId, bool includeDeleted = false)
        {
            DateTime todayDate = DateTime.UtcNow;

            if (includeDeleted)
            {
                return await _context.Orders.Include(o => o.OrderItems).Where(
                o => o.TableId == tableId && o.CreatedDate.Date == todayDate.Date)
                .ToListAsync();

            }
            else
            {
                return await _context.Orders.Include(o => o.OrderItems).Where(
                    o => o.TableId == tableId && o.CreatedDate.Date == todayDate.Date && !o.IsDeleted)
                    .ToListAsync();
            }
        }

        public async Task<IReadOnlyList<MOrder>> GetOrdersByDateAsync(DateTime from, DateTime? to = null, bool includeDeleted = false)
        {
            DateTime endDate = to ?? DateTime.UtcNow;

            if (includeDeleted)
            {
                return await _context.Orders.Include(o => o.OrderItems).Where(
                    o => o.CreatedDate.Date >= from.Date && o.CreatedDate.Date <= endDate.Date).ToListAsync();
            }
            else
            {
                return await _context.Orders.Include(o => o.OrderItems).Where(
                o => o.CreatedDate.Date >= from.Date && o.CreatedDate.Date <= endDate.Date && !o.IsDeleted).ToListAsync();
            }
        }

        public async Task<IReadOnlyList<MOrder>> GetOrdersByAssignedEmployeeAsync(Guid employeeId, bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return await _context.Orders.Include(o => o.OrderItems).Where(
                    o => o.AssignedWaiterId == employeeId || o.AssignedBaristaId == employeeId || o.AssignedChefId == employeeId ||
                    o.AssignedCashierId == employeeId).ToListAsync();
            }
            else
            {
                return await _context.Orders.Include(o => o.OrderItems).Where(
                   o => (o.AssignedWaiterId == employeeId || o.AssignedBaristaId == employeeId || o.AssignedChefId == employeeId ||
                   o.AssignedCashierId == employeeId) && !o.IsDeleted).ToListAsync();
            }
        }
    }
}
