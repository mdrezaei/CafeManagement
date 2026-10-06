using MenuAndOrdering.API.Domain.Entities;

namespace MenuAndOrdering.API.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task AddCustomerAsync(Customer customer);

        Task UpdateCustomerAsync(Customer customer);

        Task<IReadOnlyList<Customer>> GetAllCustomersAsync();

        Task<IReadOnlyList<Customer>> GetCustomersByDateAsync(DateTime from, DateTime? to = null);

        Task<Customer?> GetCustomerByIdAsync(Guid id);

        Task<Customer?> GetCustomerByPhoneNumberAsync(string phoneNumber);

        Task<IReadOnlyList<Customer>> GetCustomerByNameAsync(string name);

       

    }
}
