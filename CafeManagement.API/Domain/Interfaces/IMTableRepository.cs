using CafeManagement.API.Domain.Entities;

namespace CafeManagement.API.Domain.Interfaces
{
    public interface IMTableRepository
    {
        Task AddTableAsync(MTable table);

        Task UpdateTableAsync(MTable table);

        Task<IReadOnlyList<MTable>> GetAllTablesAsync(bool includeDeleted = false);

        Task<MTable?> GetTableByIdAsync(int id, bool includeDeleted = false);

        Task<MTable?> GetTableByTableNumberAsync(string tableNumber, bool includeDeleted = false);
    }
}
 