using MenuAndOrdering.API.Domain.Entities;

namespace MenuAndOrdering.API.Domain.Interfaces
{
    public interface IMenuItemRepository
    {
        Task AddMenuItemAsync(MenuItem menuItem);

        Task UpdateMenuItemAsync(MenuItem menuItem);

        Task<IReadOnlyList<MenuItem>> GetAllMenuItemsAsync();

        Task<MenuItem?> GetMenuItemByIdAsync(Guid id);

        Task<IReadOnlyList<MenuItem>> GetMenuItemByNameAsync(string name);

        Task<IReadOnlyList<MenuItem>> GetMenuItemsByCategoryAsync(string category);

        Task<IReadOnlyList<MenuItem>> GetAvailableMenuItemsAsync();

        Task<IReadOnlyList<MenuItem>> GetActiveMenuItemsAsync();

        


    } 
}
