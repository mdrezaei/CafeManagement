
using CafeManagement.Shared.Events;
using MenuAndOrdering.API.Domain.Entities;
using MenuAndOrdering.API.Domain.Interfaces;
using MenuAndOrdering.API.Infrastructure.Persistence;
using MenuAndOrdering.API.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MenuAndOrdering.API.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOutboxService _outboxService;

        public CustomerService(ICustomerRepository customerRepository, IOutboxService outboxService)
        {
            _customerRepository = customerRepository;
            _outboxService = outboxService;
        }

        public async Task<Customer?> GetCustomerByPhoneNumberAsync(string phoneNumber, bool isDeleted = false)
        {
            //when isDeleted argumant is true = we want deleted and non deleted
            //when isDeleted argumant is false = we want non deleted

            Customer? customer = await _customerRepository.GetCustomerByPhoneNumberAsync(phoneNumber);

            if (customer == null)
            {
                return null;
            }

            if (!isDeleted && customer.IsDeleted)
            {
                return null;
            }

            return customer;

        }

        public async Task<Customer?> GetCustomerByIdAsync(Guid id, bool isDeleted = false)
        {
            //when isDeleted argumant is true = we want deleted and non deleted
            //when isDeleted argumant is false = we want non deleted

            Customer? customer = await _customerRepository.GetCustomerByIdAsync(id);

            if (customer == null)
            {
                return null;
            }

            if (!isDeleted && customer.IsDeleted)
            {
                return null;
            }

            return customer;
        }

        public async Task<IReadOnlyList<Customer>> GetAllCustomersAsync()
        {
            //when isDeleted argumant is true = we want deleted and non deleted
            //when isDeleted argumant is false = we want non deleted

            IReadOnlyList<Customer> customers = await _customerRepository.GetAllCustomersAsync();

            return customers;

        }

        public async Task<IReadOnlyList<Customer>> GetCustomersByDateAsync(DateTime from, DateTime? to = null, bool isDeleted = false)
        {
            IReadOnlyList<Customer> customers = await _customerRepository.GetCustomersByDateAsync(from, to);

            if (isDeleted)
            {
                return customers;
            }
            else
            {
                return customers.Where(c => !c.IsDeleted).ToList();
            }

        }

        public async Task<IReadOnlyList<Customer>> GetCustomerByNameAsync(string name, bool isDeleted = false)
        {
            IReadOnlyList<Customer> customers = await _customerRepository.GetCustomerByNameAsync(name);

            if (isDeleted)
            {
                return customers;
            }
            else
            {
                return customers.Where(c => !c.IsDeleted).ToList();
            }
        }

        public async Task<Customer> CreateCustomerAsync(string name, string phoneNumber, Guid id)
        {
            Customer? existing = await _customerRepository.GetCustomerByPhoneNumberAsync(phoneNumber);

            if (existing != null)
            {
                // اگه حذف شده بود، دوباره فعالش کن
                //از انتیتی ی متد بساز که تغیرش بده به حذف نشده
                //اپدیت کن
                return existing;
            }
            Customer customer = new Customer(name, phoneNumber, id);
            await _customerRepository.AddCustomerAsync(customer);

            await _outboxService.AddMessageAsync(new CustomerCreatedEvent
            {
                CustomerId = customer.Id,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                CreatedAt = DateTime.UtcNow
            });

            return customer;
        }

        public async Task<Customer> CreateCustomerAsync(string name, string phoneNumber)
        {
            Customer? existing = await _customerRepository.GetCustomerByPhoneNumberAsync(phoneNumber);

            if (existing != null)
            {
                // اگه حذف شده بود، دوباره فعالش کن
                //از انتیتی ی متد بساز که تغیرش بده به حذف نشده
                //اپدیت کن
                return existing;
            }
            Customer customer = new Customer(name, phoneNumber);
            await _customerRepository.AddCustomerAsync(customer);

            await _outboxService.AddMessageAsync(new CustomerCreatedEvent
            {
                CustomerId = customer.Id,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                CreatedAt = DateTime.UtcNow
            });

            return customer;
        }

        //public async Task<Customer> CreateCustomerWithPasswordAsync(string name, string phoneNumber, string password)
        //{
        //    Customer customer = await CreateCustomerAsync(name, phoneNumber);
        //    customer.SetPassword(password);
        //    await _customerRepository.UpdateCustomerAsync(customer);

        //    await PublishCustomerUpdated(customer);

        //    return customer;
        //}

        public async Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            await _customerRepository.UpdateCustomerAsync(customer);

            await PublishCustomerUpdated(customer);

            return customer;
        }

        public async Task<Customer> UpdateCustomerNameAsync(Customer customer, string name)
        {
            customer.UpdateName(name);
            await _customerRepository.UpdateCustomerAsync(customer);

            await PublishCustomerUpdated(customer);

            return customer;
        }

        public async Task<Customer> UpdateCustomerPhoneNumberAsync(Customer customer, string phoneNumber)
        {
            customer.UpdatePhoneNumber(phoneNumber);
            await _customerRepository.UpdateCustomerAsync(customer);

            await PublishCustomerUpdated(customer);

            return customer;
        }

        public async Task SoftDeleteCustomerAsync(Customer customer)
        {
            customer.SoftDelete();
            await _customerRepository.UpdateCustomerAsync(customer);

            await _outboxService.AddMessageAsync(new CustomerDeletedEvent
            {
                CustomerId = customer.Id,
                DeletedAt = DateTime.UtcNow
            });
        }


        private async Task PublishCustomerUpdated(Customer customer)
        {
            await _outboxService.AddMessageAsync(new CustomerUpdatedEvent
            {
                CustomerId = customer.Id,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                UpdatedAt = DateTime.UtcNow,
            });
        }

    }
}
