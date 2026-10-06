using MenuAndOrdering.API.Domain.Entities;
using MenuAndOrdering.API.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MenuAndOrdering.API.Infrastructure.Persistence.Repositories
{
    public class TableRepository:ITableRepository
    {
        private readonly MenuAndOrderingDbContext _context;

        public TableRepository(MenuAndOrderingDbContext context)
        {
            _context = context;
        }

        public async Task AddTableAsync(Table table)
        {
            await _context.Tables.AddAsync(table);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTableAsync(Table table)
        {
            _context.Tables.Update(table);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Table>> GetAllTablesAsync()
        {
            return await _context.Tables.Where(t => !t.IsDeleted).ToListAsync();
        }

        public async Task<Table?> GetTableByIdAsync(int id)
        {
            return await _context.Tables.FindAsync(id);
        }

        public async Task<Table?> GetTableByTableNumberAsync(string tableName)
        {
            return await _context.Tables.FirstOrDefaultAsync(t => t.TableNumber == tableName);
        }
    }
}
