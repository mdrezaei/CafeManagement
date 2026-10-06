using MenuAndOrdering.API.Domain.Entities;

namespace MenuAndOrdering.API.Application.Services
{
    public interface ICustomerService
    {
        Task<Customer?> GetCustomerByPhoneNumberAsync(string phoneNumber, bool isDeleted = false);
        Task<Customer?> GetCustomerByIdAsync(Guid id, bool isDeleted = false);
        Task<IReadOnlyList<Customer>> GetAllCustomersAsync(); 
        Task<IReadOnlyList<Customer>> GetCustomersByDateAsync(DateTime from, DateTime? to = null, bool isDeleted = false);
        Task<IReadOnlyList<Customer>> GetCustomerByNameAsync(string name, bool isDeleted = false);

        Task<Customer> CreateCustomerAsync(string name, string phoneNumber, Guid id);
        Task<Customer> CreateCustomerAsync(string name, string phoneNumber);
        //Task<Customer> CreateCustomerWithPasswordAsync(string name, string phoneNumber, string password);
        Task<Customer> UpdateCustomerAsync(Customer customer);
        Task<Customer> UpdateCustomerNameAsync(Customer customer, string name);
        Task<Customer> UpdateCustomerPhoneNumberAsync(Customer customer, string phoneNumber);
        Task SoftDeleteCustomerAsync(Customer customer);

    }
}
