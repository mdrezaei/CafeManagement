using CafeManagement.API.Domain.Entities;
using CafeManagement.API.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CafeManagement.API.Infrastructure.Persistence.Repositories
{
    public class MTableRepository : IMTableRepository
    {
        private readonly CafeManagementDbContext _context;

        public MTableRepository(CafeManagementDbContext context)
        {
            _context = context;
        }

        public async Task AddTableAsync(MTable table)
        {
            await _context.Tables.AddAsync(table);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTableAsync(MTable table)
        {
            _context.Tables.Update(table);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<MTable>> GetAllTablesAsync(bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return await _context.Tables.Where(t => !t.IsDeleted || t.IsDeleted).ToListAsync();
            }
            else
            {
                return await _context.Tables.Where(t => !t.IsDeleted).ToListAsync();
            }
        }

        public async Task<MTable?> GetTableByIdAsync(int id, bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return await _context.Tables.FindAsync(id);
            }
            else
            {
                return await _context.Tables.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
            }
        }

        public async Task<MTable?> GetTableByTableNumberAsync(string tableNumber, bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return await _context.Tables.FirstOrDefaultAsync(t => t.TableNumber == tableNumber);
            }
            else
            {

                return await _context.Tables.FirstOrDefaultAsync(
                    t => t.TableNumber.ToLower() == tableNumber.ToLower() && !t.IsDeleted);
            }
        }
    }
}
