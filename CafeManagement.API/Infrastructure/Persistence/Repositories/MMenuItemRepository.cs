using CafeManagement.API.Domain.Entities;
using CafeManagement.API.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CafeManagement.API.Infrastructure.Persistence.Repositories
{
    public class MMenuItemRepository : IMMenuItemRepository
    {
        private readonly CafeManagementDbContext _context;
        public MMenuItemRepository(CafeManagementDbContext context)
        {
            _context = context;
        }

        public async Task AddMenuItemAsync(MMenuItem menuItem)
        {
            await _context.MenuItems.AddAsync(menuItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMenuItemAsync(MMenuItem menuItem)
        {
            _context.MenuItems.Update(menuItem);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<MMenuItem>> GetAllMenuItemsAsync(bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return await _context.MenuItems.ToListAsync();
            }
            else
            {
                return await _context.MenuItems.Where(m => !m.IsDeleted).ToListAsync();
            }
        }

        public async Task<MMenuItem?> GetMenuItemByIdAsync(Guid id, bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return await _context.MenuItems.FindAsync(id);
            }
            else
            {
                return await _context.MenuItems.FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
            }
        }

        public async Task<IReadOnlyList<MMenuItem>> GetMenuItemByNameAsync(string name, bool includeDeleted = false)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("اسم خالیست.");
            }

            if (includeDeleted)
            {
                return await _context.MenuItems.Where(m => m.Name.Contains(name)).ToListAsync();
            }
            else
            {
                return await _context.MenuItems.Where(m => m.Name.Contains(name) && !m.IsDeleted).ToListAsync();
            }
        }

        public async Task<IReadOnlyList<MMenuItem>> GetMenuItemsByCategoryAsync(string category, bool includeDeleted = false)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentException("دسته بندی خالیست.");
            }

            if (includeDeleted)
            {
                return await _context.MenuItems.Where(m => m.Category == category ).ToListAsync();
            }
            else
            {
                return await _context.MenuItems.Where(m => m.Category == category && !m.IsDeleted ).ToListAsync();
            }
        }

        public async Task<IReadOnlyList<MMenuItem>> GetAvailableMenuItemsAsync(bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return await _context.MenuItems.Where(m => m.IsAvailable).ToListAsync();
            }
            else
            {
                return await _context.MenuItems.Where(m => m.IsAvailable && !m.IsDeleted).ToListAsync();
            }
        }

        public async Task<IReadOnlyList<MMenuItem>> GetActiveMenuItemsAsync(bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return await _context.MenuItems.Where(m => m.IsActive).ToListAsync();
            }
            else
            {
                return await _context.MenuItems.Where(m => m.IsActive && !m.IsDeleted).ToListAsync();
            }
        }
    }
}
