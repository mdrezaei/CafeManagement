using CafeManagement.API.Domain.Entities;

namespace CafeManagement.API.Domain.Interfaces
{
    public interface IMMenuItemRepository
    {
        Task AddMenuItemAsync(MMenuItem menuItem);

        Task UpdateMenuItemAsync(MMenuItem menuItem);

        Task<IReadOnlyList<MMenuItem>> GetAllMenuItemsAsync(bool includeDeleted = false);

        Task<MMenuItem?> GetMenuItemByIdAsync(Guid id, bool includeDeleted = false);

        Task<IReadOnlyList<MMenuItem>> GetMenuItemByNameAsync(string name, bool includeDeleted = false);

        Task<IReadOnlyList<MMenuItem>> GetMenuItemsByCategoryAsync(string category, bool includeDeleted = false);

        Task<IReadOnlyList<MMenuItem>> GetAvailableMenuItemsAsync(bool includeDeleted = false);

        Task<IReadOnlyList<MMenuItem>> GetActiveMenuItemsAsync(bool includeDeleted = false);
    }
}
