using CafeManagement.API.Domain.Entities;
using CafeManagement.API.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CafeManagement.API.Infrastructure.Persistence.Repositories
{
    public class MCustomerRepository : IMCustomerRepository
    {
        private readonly CafeManagementDbContext _context;
        public MCustomerRepository(CafeManagementDbContext context)
        {
            _context = context;
        }

        public async Task AddCustomerAsync(MCustomer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(MCustomer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

        }

        public async Task<IReadOnlyList<MCustomer>> GetAllCustomersAsync(bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return await _context.Customers.ToListAsync();
            }
            else
            {
                return await _context.Customers.Where(c => !c.IsDeleted).ToListAsync();
            }
        }

        public async Task<IReadOnlyList<MCustomer>> GetCustomersByDateAsync(DateTime from, DateTime? to = null, bool includeDeleted = false)
        {
            DateTime endDate = to ?? DateTime.UtcNow;

            if (includeDeleted)
            {
                return await _context.Customers.Where(
                    c => c.SubmitDate.Date >= from.Date && c.SubmitDate.Date <= endDate.Date).ToListAsync();
            }
            else
            {
                return await _context.Customers.Where(
                    c => c.SubmitDate.Date >= from.Date && c.SubmitDate.Date <= endDate.Date && !c.IsDeleted).ToListAsync();
            }

        }

        public async Task<MCustomer?> GetCustomerByIdAsync(Guid id, bool includeDeleted = false)
        {
            MCustomer? customer = await _context.Customers.FindAsync(id);

            if (includeDeleted)
            {
                return await _context.Customers.FindAsync(id);
            }
            else
            {
                return await _context.Customers.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
            }

        }

        public async Task<MCustomer?> GetCustomerByPhoneNumberAsync(string phoneNumber, bool includeDeleted = false)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentException("شماره تلفن خالیست.");
            }
            
            if (includeDeleted)
            {
                return await _context.Customers.FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber);
            }
            else
            {
                return await _context.Customers.FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber && !c.IsDeleted);
            }

        }

        public async Task<IReadOnlyList<MCustomer>> GetCustomerByNameAsync(string name, bool includeDeleted = false)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("اسم خالیست.");
            }

            if (includeDeleted)
            {
                return await _context.Customers.Where(c => c.Name.Contains(name)).ToListAsync();
            }
            else
            {
                return await _context.Customers.Where(c => c.Name.Contains(name) && !c.IsDeleted).ToListAsync();
            }

        }
    }
}
