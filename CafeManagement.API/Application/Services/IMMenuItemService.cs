using CafeManagement.API.Domain.Entities;

namespace CafeManagement.API.Application.Services
{
    public interface IMMenuItemService
    {
        Task<MMenuItem> CreateMenuItemAsync(string name, decimal price, string category);
        Task<MMenuItem> UpdateMenuItemAsync(MMenuItem menuItem);
        Task<MMenuItem> SetAvailableAsync(MMenuItem menuItem);
        Task<MMenuItem> SetUnavailableAsync(MMenuItem menuItem);
        Task<MMenuItem> ActivateAsync(MMenuItem menuItem);
        Task<MMenuItem> DeactivateAsync(MMenuItem menuItem);
        Task<MMenuItem> UpdateMenuItemNameAsync(MMenuItem menuItem, string name);
        Task<MMenuItem> UpdateMenuItemPriceAsync(MMenuItem menuItem, decimal price);
        Task<MMenuItem> UpdateMenuItemDescriptionAsync(MMenuItem menuItem, string? description);
        Task<MMenuItem> DeleteMenuItemDescriptionAsync(MMenuItem menuItem);
        Task<MMenuItem> AddMenuItemThumbNailUrlAsync(MMenuItem menuItem, string? thumbNailUrl);
        Task<MMenuItem> SetMenuItemThumbNailUrlAsync(MMenuItem menuItem, List<string>? picturesUrl, int index);
        Task<MMenuItem> UpdateMenuItemThumbNailUrlAsync(MMenuItem menuItem, string? thumbNailUrl);
        Task<MMenuItem> DeleteMenuItemThumbNailUrlAsync(MMenuItem menuItem);
        Task<MMenuItem> AddMenuItemPicturesUrlAsync(MMenuItem menuItem, List<string>? picturesUrl);
        Task<MMenuItem> UpdateMenuItemPicturesUrlAsync(MMenuItem menuItem, string pUrl, int? i = null);
        Task<MMenuItem> DeleteMenuItemPicturesUrlAsync(MMenuItem menuItem, int index);
        Task<MMenuItem> UpdateMenuItemCategoryAsync(MMenuItem menuItem, string category);
        Task SoftDeleteMenuItemAsync(MMenuItem menuItem);
        Task<IReadOnlyList<MMenuItem>> GetAllMenuItemsAsync(bool includeDeleted = false);
        Task<MMenuItem?> GetMenuItemByIdAsync(Guid id, bool includeDeleted = false);
        Task<IReadOnlyList<MMenuItem>> GetMenuItemByNameAsync(string name, bool includeDeleted = false);
        Task<IReadOnlyList<MMenuItem>> GetMenuItemsByCategoryAsync(string category, bool includeDeleted = false);
        Task<IReadOnlyList<MMenuItem>> GetAvailableMenuItemsAsync(bool includeDeleted = false);
        Task<IReadOnlyList<MMenuItem>> GetActiveMenuItemsAsync(bool includeDeleted = false);

    }
}
