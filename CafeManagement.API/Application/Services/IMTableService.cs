using CafeManagement.API.Domain.Entities;

namespace CafeManagement.API.Application.Services
{
    public interface IMTableService
    {
        Task<MTable> CreateTableAsync(string tableNumber);
        Task<MTable> UpdateTableAsync(MTable table);
        Task<MTable> UpdateTableNumberAsync(MTable table, string tableNumber);
        Task SoftDeleteTableAsync(MTable table);
        Task<IReadOnlyList<MTable>> GetAllTablesAsync(bool includeDeleted = false);
        Task<MTable?> GetTableByIdAsync(int id, bool includeDeleted = false);
        Task<MTable?> GetTableByTableNumberAsync(string tableNumber, bool includeDeleted = false);

    }
}
