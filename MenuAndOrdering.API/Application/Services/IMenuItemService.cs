using MenuAndOrdering.API.Domain.Entities;

namespace MenuAndOrdering.API.Application.Services
{
    public interface IMenuItemService
    {
        Task<MenuItem> CreateMenuItemAsync(string name, string category, decimal price, string? description, Guid id);
        Task<MenuItem> CreateMenuItemAsync(string name, string category, decimal price, string? description);
        Task<MenuItem> UpdateMenuItemAsync(MenuItem menuItem);

        Task<MenuItem> SetAvailableAsync(MenuItem menuItem);
        Task<MenuItem> SetUnavailableAsync(MenuItem menuItem);
        Task<MenuItem> ActivateAsync(MenuItem menuItem);
        Task<MenuItem> DeactivateAsync(MenuItem menuItem);
        Task<MenuItem> UpdateMenuItemNameAsync(MenuItem menuItem, string name);
        Task<MenuItem> UpdateMenuItemPriceAsync(MenuItem menuItem, decimal price);
        Task<MenuItem> UpdateMenuItemDescriptionAsync(MenuItem menuItem, string? description);
        Task<MenuItem> DeleteMenuItemDescriptionAsync(MenuItem menuItem);
        Task<MenuItem> AddMenuItemThumbNailUrlAsync(MenuItem menuItem, string? thumbNailUrl);
        Task<MenuItem> SetMenuItemThumbNailUrlAsync(MenuItem menuItem, List<string>? picturesUrl, int index);
        Task<MenuItem> UpdateMenuItemThumbNailUrlAsync(MenuItem menuItem, string? thumbNailUrl);
        Task<MenuItem> DeleteMenuItemThumbNailUrlAsync(MenuItem menuItem);
        Task<MenuItem> AddMenuItemPicturesUrlAsync(MenuItem menuItem, List<string>? picturesUrl);
        Task<MenuItem> UpdateMenuItemPicturesUrlAsync(MenuItem menuItem, string pUrl, int? i = null);
        Task<MenuItem> DeleteMenuItemPicturesUrlAsync(MenuItem menuItem, int index);
        Task<MenuItem> UpdateMenuItemCategoryAsync(MenuItem menuItem, string category);
        Task SoftDeleteMenuItemAsync(MenuItem menuItem);


        Task<MenuItem?> GetMenuItemByIdAsync(Guid id, bool isDeleted = false, bool isActive = true);
        Task<IReadOnlyList<MenuItem>> GetMenuItemsByNameAsync(string name);
        Task<IReadOnlyList<MenuItem>> GetMenuItemsByCategoryAsync(string category);
        Task<IReadOnlyList<MenuItem>> GetAllMenuItemsAsync(bool isDeleted = false);
        Task<IReadOnlyList<MenuItem>> GetAllAvailableMenuItemsAsync();
    }
}
 