using CafeManagement.Shared.Events;
using MenuAndOrdering.API.Domain.Entities;
using MenuAndOrdering.API.Domain.Interfaces;
using MenuAndOrdering.API.Infrastructure.Persistence.Repositories;

namespace MenuAndOrdering.API.Application.Services
{
    public class TableService:ITableService
    {
        private readonly ITableRepository _tableRepository;

        public TableService(ITableRepository tableRepository)
        {
            _tableRepository = tableRepository;
        }

        public async Task<Table> CreateTableAsync(string tableNumber)
        {
            Table? existing = await _tableRepository.GetTableByTableNumberAsync(tableNumber);
            if (existing != null)
            {
                // اگه حذف شده بود، دوباره فعالش کن
                //از انتیتی ی متد بساز که تغیرش بده به حذف نشده
                //اپدیت کن
                return existing;
            }

            Table table = new Table(tableNumber);
            await _tableRepository.AddTableAsync(table);
            return table;
        }


        public async Task<Table> UpdateTableNumberAsync(Table table, string tableNumber)
        {
            table.UpdateTableNumber(tableNumber);
            await _tableRepository.UpdateTableAsync(table);
            return table;
        }


        public async Task SoftDeleteTableAsync(Table table)
        {
            table.SoftDelete();
            await _tableRepository.UpdateTableAsync(table);
        }

        public async Task<Table?> GetTableByIdAsync(int id)
        {
            return await _tableRepository.GetTableByIdAsync(id);
        }

        public async Task<Table?> GetTableByTableNumberAsync(string tableName)
        {
            return await _tableRepository.GetTableByTableNumberAsync(tableName);
        }
    }
}
