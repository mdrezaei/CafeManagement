using CafeManagement.API.Domain.Entities;
using CafeManagement.API.Domain.Interfaces;
using CafeManagement.API.Infrastructure.Persistence.Repositories;
using CafeManagement.Shared.Events;

namespace CafeManagement.API.Application.Services
{
    public class MTableService : IMTableService
    {
        private readonly IMTableRepository _mTableRepository;
        private readonly IOutboxService _outboxService;

        public MTableService(IMTableRepository mTableRepository, IOutboxService outboxService)
        {
            _mTableRepository = mTableRepository;
            _outboxService = outboxService;
        }

        public async Task<MTable> CreateTableAsync(string tableNumber)
        {
            MTable? existing = await _mTableRepository.GetTableByTableNumberAsync(tableNumber);
            if (existing != null)
            {
                // اگه حذف شده بود، دوباره فعالش کن
                //از انتیتی ی متد بساز که تغیرش بده به حذف نشده
                //اپدیت کن
                return existing;
            }

            MTable table = new MTable(tableNumber);
            await _mTableRepository.AddTableAsync(table);

            await _outboxService.AddMessageAsync(new TableCreatedEvent
            {
                TableId = table.Id,
                TableNumber = table.TableNumber,
                CreatedAt = DateTime.UtcNow
            });

            return table;
        }

        public async Task<MTable> UpdateTableAsync(MTable table)
        {
            await _mTableRepository.UpdateTableAsync(table);

            await PublishTableUpdated(table);

            return table;
        }

        public async Task<MTable> UpdateTableNumberAsync(MTable table, string tableNumber)
        {
            table.UpdateTableNumber(tableNumber);
            await _mTableRepository.UpdateTableAsync(table);

            await PublishTableUpdated(table);

            return table;
        }

        public async Task SoftDeleteTableAsync(MTable table)
        {
            table.SoftDelete();
            await _mTableRepository.UpdateTableAsync(table);

            await _outboxService.AddMessageAsync(new TableDeletedEvent
            {
                TableId = table.Id,
                DeletedAt = DateTime.UtcNow
            });

        }

        public async Task<IReadOnlyList<MTable>> GetAllTablesAsync(bool includeDeleted = false)
        {
            return await _mTableRepository.GetAllTablesAsync(includeDeleted);
        }

        public async Task<MTable?> GetTableByIdAsync(int id, bool includeDeleted = false)
        {
            return await _mTableRepository.GetTableByIdAsync(id, includeDeleted);
        }

        public async Task<MTable?> GetTableByTableNumberAsync(string tableNumber, bool includeDeleted = false)
        {
            return await _mTableRepository.GetTableByTableNumberAsync(tableNumber, includeDeleted);
        }

        private async Task PublishTableUpdated(MTable table)
        {
            await _outboxService.AddMessageAsync(new TableUpdatedEvent
            {
                TableId = table.Id,
                TableNumber = table.TableNumber,
                UpdatedAt = DateTime.UtcNow
            });
        }

    }
}
