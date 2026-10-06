using CafeManagement.API.Domain.Entities;
using CafeManagement.API.Domain.Interfaces;
using CafeManagement.API.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;

namespace CafeManagement.API.Infrastructure.Persistence.Repositories
{
    public class MEmployeeRepository : IMEmployeeRepository
    {
        private readonly CafeManagementDbContext _context;
        public MEmployeeRepository(CafeManagementDbContext context)
        {
            _context = context;
        }

        public async Task AddEmployeeAsync(MEmployee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEmployeeAsync(MEmployee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<MEmployee>> GetAllEmployeesAsync(bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return await _context.Employees.ToListAsync();
            }
            else
            {
                return await _context.Employees.Where(e => !e.IsDeleted).ToListAsync();
            }

        }

        public async Task<IReadOnlyList<MEmployee>> GetEmployeesByDateAsync(DateTime from, DateTime? to = null, bool includeDeleted = false)
        {
            DateTime endDate = to ?? DateTime.UtcNow;

            if (includeDeleted)
            {
                return await _context.Employees.Where(
                    e => e.JoinedDate.Date >= from.Date && e.JoinedDate.Date <= endDate.Date).ToListAsync();
            }
            else
            {
                return await _context.Employees.Where(
                    e => e.JoinedDate.Date >= from.Date && e.JoinedDate.Date <= endDate.Date && !e.IsDeleted).ToListAsync();
            }

        }

        public async Task<MEmployee?> GetEmployeeByIdAsync(Guid id, bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return await _context.Employees.FindAsync(id);
            }
            else
            {
                return await _context.Employees.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
            }
        }

        public async Task<MEmployee?> GetEmployeeByPhoneNumberAsync(string phoneNumber, bool includeDeleted = false)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentException("شماره تلفن خالیست.");
            }

            if (includeDeleted)
            {
                return await _context.Employees.FirstOrDefaultAsync(e => e.PhoneNumber == phoneNumber);
            }
            else
            {
                return await _context.Employees.FirstOrDefaultAsync(e => e.PhoneNumber == phoneNumber && !e.IsDeleted);
            }

        }

        public async Task<IReadOnlyList<MEmployee>> GetEmployeeByNameAsync(string name, bool includeDeleted = false)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("اسم خالیست.");
            }

            if (includeDeleted)
            {
                return await _context.Employees.Where(e => e.Name.Contains(name)).ToListAsync();
            }
            else
            {
                return await _context.Employees.Where(e => e.Name.Contains(name) && !e.IsDeleted).ToListAsync();
            }

        }

        public async Task<IReadOnlyList<MEmployee>> GetEmployeesBySectionAsync(EmployeeSections section, bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return await _context.Employees.Where(e => e.Section == section || e.SecondarySection == section).ToListAsync();
            }
            else
            {
                return await _context.Employees.Where(
                    e => (e.Section == section || e.SecondarySection == section) && !e.IsDeleted).ToListAsync();
            }

        }

        public async Task<IReadOnlyList<MEmployee>> GetEmployeesByLevelAsync(EmployeeLevels level, bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return await _context.Employees.Where(e => e.Level == level || e.SecondaryLevel == level).ToListAsync();
            }
            else
            {
                return await _context.Employees.Where(
                    e => (e.Level == level || e.SecondaryLevel == level) && !e.IsDeleted).ToListAsync();
            }
        }
    }
}
