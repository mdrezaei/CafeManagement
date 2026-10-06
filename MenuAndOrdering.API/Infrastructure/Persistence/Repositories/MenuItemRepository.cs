using MenuAndOrdering.API.Domain.Entities;
using MenuAndOrdering.API.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MenuAndOrdering.API.Infrastructure.Persistence.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly MenuAndOrderingDbContext _context;

        public MenuItemRepository(MenuAndOrderingDbContext context)
        {
            _context = context;
        }

        public async Task AddMenuItemAsync(MenuItem menuItem)
        {
            await _context.MenuItems.AddAsync(menuItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMenuItemAsync(MenuItem menuItem)
        {
            _context.MenuItems.Update(menuItem);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<MenuItem>> GetAllMenuItemsAsync()
        {
            return await _context.MenuItems.Where(m => !m.IsDeleted && m.IsActive).ToListAsync();
        }

        public async Task<MenuItem?> GetMenuItemByIdAsync(Guid id)
        {
            return await _context.MenuItems.FindAsync(id);
        }

        public async Task<IReadOnlyList<MenuItem>> GetMenuItemByNameAsync(string name)
        {
            return await _context.MenuItems.Where(m => m.Name.Contains(name) && !m.IsDeleted && m.IsActive).ToListAsync();
        }

        public async Task<IReadOnlyList<MenuItem>> GetMenuItemsByCategoryAsync(string category)
        {
            return await _context.MenuItems.Where(m => m.Category == category && !m.IsDeleted && m.IsActive).ToListAsync();
        }

        public async Task<IReadOnlyList<MenuItem>> GetAvailableMenuItemsAsync()
        {
            return await _context.MenuItems.Where(m => m.IsAvailable).ToListAsync();
        }

        public async Task<IReadOnlyList<MenuItem>> GetActiveMenuItemsAsync()
        {
            return await _context.MenuItems.Where(m => m.IsActive).ToListAsync();
        }
    }
}
