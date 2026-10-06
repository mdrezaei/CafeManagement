using CafeManagement.API.Domain.Entities;

namespace CafeManagement.API.Application.Services
{
    public interface IMCustomerService
    {
        Task<MCustomer> CreateCustomerAsync(string name, string phoneNumber, Guid id);
        Task<MCustomer> CreateCustomerAsync(string name, string phoneNumber);
        Task<MCustomer> CreateCustomerWithPasswordAsync(string name, string phoneNumber, string password);
        Task<MCustomer> SetCustomerPasswordAsync(MCustomer customer, string password); 
        Task<MCustomer> UpdateCustomerAsync(MCustomer customer); 
        Task<MCustomer> UpdateCustomerNameAsync(MCustomer customer, string name);
        Task<MCustomer> UpdateCustomerPhoneNumberAsync(MCustomer customer, string phoneNumber);
        Task<MCustomer> UpdateCustomerPasswordAsync(MCustomer customer, string password);
        Task SoftDeleteCustomerAsync(MCustomer customer);
        Task<IReadOnlyList<MCustomer>> GetAllCustomersAsync(bool includeDeleted = false);
        Task<MCustomer?> GetCustomerByIdAsync(Guid id, bool includeDeleted = false);
        Task<MCustomer?> GetCustomerByPhoneNumberAsync(string phoneNumber, bool includeDeleted = false);
        Task<IReadOnlyList<MCustomer>> GetCustomerByNameAsync(string name, bool includeDeleted = false);
        Task<IReadOnlyList<MCustomer>> GetCustomerByDateAsync(DateTime from, DateTime? to = null, bool includeDeleted = false);
         
    }
}
