using CafeManagement.API.Domain.Entities;

namespace CafeManagement.API.Domain.Interfaces
{
    public interface IMCustomerRepository
    {
        Task AddCustomerAsync(MCustomer customer);

        Task UpdateCustomerAsync(MCustomer customer);

        Task<IReadOnlyList<MCustomer>> GetAllCustomersAsync(bool includeDeleted = false);

        Task<IReadOnlyList<MCustomer>> GetCustomersByDateAsync(DateTime from, DateTime? to = null, bool includeDeleted = false);

        Task<MCustomer?> GetCustomerByIdAsync(Guid id, bool includeDeleted = false);

        Task<MCustomer?> GetCustomerByPhoneNumberAsync(string phoneNumber, bool includeDeleted = false);

        Task<IReadOnlyList<MCustomer>> GetCustomerByNameAsync(string name, bool includeDeleted = false);

    }
}
