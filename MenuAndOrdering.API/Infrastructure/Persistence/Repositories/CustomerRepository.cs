using MenuAndOrdering.API.Domain.Entities;
using MenuAndOrdering.API.Domain.Interfaces;
using MenuAndOrdering.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MenuAndOrdering.API.Infrastructure.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly MenuAndOrderingDbContext _context;

        public CustomerRepository(MenuAndOrderingDbContext context)
        {
            _context = context;
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }
         
        public async Task<IReadOnlyList<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.Where(c => !c.IsDeleted).ToListAsync();
        }

        public async Task<IReadOnlyList<Customer>> GetCustomersByDateAsync(DateTime from, DateTime? to = null)
        {
            DateTime endDate = to ?? DateTime.UtcNow;
            return await _context.Customers.Where(
                c => c.SubmitDate.Date >= from.Date && c.SubmitDate.Date <= endDate.Date)
                .ToListAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(Guid id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer?> GetCustomerByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber);
        }

        public async Task<IReadOnlyList<Customer>> GetCustomerByNameAsync(string name)
        {
            return await _context.Customers.Where(c => c.Name.Contains(name)).ToListAsync();
        }
    }
}
