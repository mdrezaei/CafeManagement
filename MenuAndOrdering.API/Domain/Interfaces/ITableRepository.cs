using MenuAndOrdering.API.Domain.Entities;
using System.Collections;

namespace MenuAndOrdering.API.Domain.Interfaces
{
    public interface ITableRepository
    {
        Task AddTableAsync(Table table);

        Task UpdateTableAsync(Table table);

        Task<IReadOnlyList<Table>> GetAllTablesAsync();

        Task<Table?> GetTableByIdAsync(int id);

        //TableNumber is the name of table
        Task<Table?> GetTableByTableNumberAsync(string tableName);




    }
}
