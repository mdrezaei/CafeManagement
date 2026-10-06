using MenuAndOrdering.API.Domain.Entities;

namespace MenuAndOrdering.API.Application.Services
{
    public interface ITableService
    {
        Task<Table?> GetTableByIdAsync(int id);

        //TableNumber is the name of table
        Task<Table?> GetTableByTableNumberAsync(string tableName);

        Task<Table> CreateTableAsync(string tableNumber);
        Task<Table> UpdateTableNumberAsync(Table table, string tableNumber);
        Task SoftDeleteTableAsync(Table table);
    }
}
