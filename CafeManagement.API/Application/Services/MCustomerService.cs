using CafeManagement.API.Domain.Entities;
using CafeManagement.API.Domain.Interfaces;
using CafeManagement.Shared.Events;

namespace CafeManagement.API.Application.Services
{
    public class MCustomerService : IMCustomerService
    {
        private readonly IMCustomerRepository _mCustomerRepository;
        private readonly IOutboxService _outboxService;
        
        public MCustomerService(IMCustomerRepository mCustomerRepository, IOutboxService outboxService)
        {
            _mCustomerRepository = mCustomerRepository;
            _outboxService = outboxService;
        }


        public async Task<MCustomer> CreateCustomerAsync(string name, string phoneNumber, Guid id)
        {
            MCustomer? existing = await _mCustomerRepository.GetCustomerByPhoneNumberAsync(phoneNumber);
            if (existing != null)
            {
                // اگه حذف شده بود، دوباره فعالش کن
                //از انتیتی ی متد بساز که تغیرش بده به حذف نشده
                //اپدیت کن
                return existing;
            }

            MCustomer customer = new MCustomer(name, phoneNumber, id);
            await _mCustomerRepository.AddCustomerAsync(customer);

            await _outboxService.AddMessageAsync(new CustomerCreatedEvent
            {
                CustomerId = customer.Id,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
            });

            return customer;
        }

        public async Task<MCustomer> CreateCustomerAsync(string name, string phoneNumber)
        {
            MCustomer? existing = await _mCustomerRepository.GetCustomerByPhoneNumberAsync(phoneNumber);
            if (existing != null)
            {
                // اگه حذف شده بود، دوباره فعالش کن
                //از انتیتی ی متد بساز که تغیرش بده به حذف نشده
                //اپدیت کن
                return existing;
            }

            MCustomer customer = new MCustomer(name, phoneNumber);
            await _mCustomerRepository.AddCustomerAsync(customer);

            await _outboxService.AddMessageAsync(new CustomerCreatedEvent
            {
                CustomerId = customer.Id,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
            });

            return customer;
        }

        public async Task<MCustomer> CreateCustomerWithPasswordAsync(string name, string phoneNumber, string password)
        {
            MCustomer customer = await CreateCustomerAsync(name, phoneNumber);
            customer.SetPassword(password);
            await _mCustomerRepository.UpdateCustomerAsync(customer);

            await PublishCustomerUpdated(customer);

            return customer;
        }

        public async Task<MCustomer> SetCustomerPasswordAsync(MCustomer customer, string password)
        {
            customer.SetPassword(password);
            await _mCustomerRepository.UpdateCustomerAsync(customer);
            
            await PublishCustomerUpdated(customer);
            
            return customer;
        }

        public async Task<MCustomer> UpdateCustomerAsync(MCustomer customer)
        {
            await _mCustomerRepository.UpdateCustomerAsync(customer);

            await PublishCustomerUpdated(customer);

            return customer;
        }

        public async Task<MCustomer> UpdateCustomerNameAsync(MCustomer customer, string name)
        {
            customer.UpdateName(name);
            await _mCustomerRepository.UpdateCustomerAsync(customer);

            await PublishCustomerUpdated(customer);

            return customer;
        }

        public async Task<MCustomer> UpdateCustomerPhoneNumberAsync(MCustomer customer, string phoneNumber)
        {
            customer.UpdatePhoneNumber(phoneNumber);
            await _mCustomerRepository.UpdateCustomerAsync(customer);

            await PublishCustomerUpdated(customer);

            return customer;
        }

        public async Task<MCustomer> UpdateCustomerPasswordAsync(MCustomer customer, string password)
        {
            customer.UpdatePassword(password);
            await _mCustomerRepository.UpdateCustomerAsync(customer);

            await PublishCustomerUpdated(customer);

            return customer;
        }

        public async Task SoftDeleteCustomerAsync(MCustomer customer)
        {
            customer.SoftDelete();
            await _mCustomerRepository.UpdateCustomerAsync(customer);

            await _outboxService.AddMessageAsync(new CustomerDeletedEvent
            {
                CustomerId = customer.Id,
                DeletedAt = DateTime.UtcNow
            });
        }

        public async Task<IReadOnlyList<MCustomer>> GetAllCustomersAsync(bool includeDeleted = false)
        {
            return await _mCustomerRepository.GetAllCustomersAsync(includeDeleted);
        }

        public async Task<MCustomer?> GetCustomerByIdAsync(Guid id, bool includeDeleted = false)
        {
            return await _mCustomerRepository.GetCustomerByIdAsync(id, includeDeleted);
        }

        public async Task<MCustomer?> GetCustomerByPhoneNumberAsync(string phoneNumber, bool includeDeleted = false)
        {
            return await _mCustomerRepository.GetCustomerByPhoneNumberAsync(phoneNumber, includeDeleted);
        }

        public async Task<IReadOnlyList<MCustomer>> GetCustomerByNameAsync(string name, bool includeDeleted = false)
        {
            return await _mCustomerRepository.GetCustomerByNameAsync(name, includeDeleted);
        }

        public async Task<IReadOnlyList<MCustomer>> GetCustomerByDateAsync(DateTime from, DateTime? to = null, bool includeDeleted = false)
        {
            return await _mCustomerRepository.GetCustomersByDateAsync(from, to, includeDeleted);
        }

        private async Task PublishCustomerUpdated(MCustomer customer)
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
