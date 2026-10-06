using CafeManagement.Shared.Events;
using MenuAndOrdering.API.Domain.Entities;
using MenuAndOrdering.API.Domain.Interfaces;
using MenuAndOrdering.API.Infrastructure.Persistence.Repositories;

namespace MenuAndOrdering.API.Application.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;

        public MenuItemService(IMenuItemRepository menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        public async Task<MenuItem> CreateMenuItemAsync(string name, string category, decimal price, string? description, Guid id)
        {
            MenuItem menuItem = new MenuItem(name, category, price, id);
            menuItem.UpdateInfo(name, description, category, price);
            await _menuItemRepository.AddMenuItemAsync(menuItem);
            return menuItem;
        }
        public async Task<MenuItem> CreateMenuItemAsync(string name, string category, decimal price, string? description)
        {
            MenuItem menuItem = new MenuItem(name, category, price);
            menuItem.UpdateInfo(name, description, category, price);
            await _menuItemRepository.AddMenuItemAsync(menuItem);
            return menuItem;
        }

        public async Task<MenuItem> UpdateMenuItemAsync(MenuItem menuItem)
        {
            await _menuItemRepository.UpdateMenuItemAsync(menuItem);
            return menuItem;
        }

        public async Task<MenuItem> SetAvailableAsync(MenuItem menuItem)
        {
            menuItem.SetAvailable();
            await _menuItemRepository.UpdateMenuItemAsync(menuItem);
            return menuItem;
        }

        public async Task<MenuItem> SetUnavailableAsync(MenuItem menuItem)
        {
            menuItem.SetUnavailable();
            await _menuItemRepository.UpdateMenuItemAsync(menuItem);
            return menuItem;
        }

        public async Task<MenuItem> ActivateAsync(MenuItem menuItem)
        {
            menuItem.Activate();
            await _menuItemRepository.UpdateMenuItemAsync(menuItem);
            return menuItem;
        }

        public async Task<MenuItem> DeactivateAsync(MenuItem menuItem)
        {
            menuItem.Deactivate();
            await _menuItemRepository.UpdateMenuItemAsync(menuItem);
            return menuItem;
        }

        public async Task<MenuItem> UpdateMenuItemNameAsync(MenuItem menuItem, string name)
        {
            menuItem.UpdateName(name);
            await _menuItemRepository.UpdateMenuItemAsync(menuItem);
            return menuItem;
        }

        public async Task<MenuItem> UpdateMenuItemPriceAsync(MenuItem menuItem, decimal price)
        {
            menuItem.UpdatePrice(price);
            await _menuItemRepository.UpdateMenuItemAsync(menuItem);
            return menuItem;
        }

        public async Task<MenuItem> UpdateMenuItemDescriptionAsync(MenuItem menuItem, string? description)
        {
            menuItem.UpdateDescription(description);
            await _menuItemRepository.UpdateMenuItemAsync(menuItem);
            return menuItem;
        }

        public async Task<MenuItem> DeleteMenuItemDescriptionAsync(MenuItem menuItem)
        {
            menuItem.DeleteDescription();
            await _menuItemRepository.UpdateMenuItemAsync(menuItem);
            return menuItem;
        }

        public async Task<MenuItem> AddMenuItemThumbNailUrlAsync(MenuItem menuItem, string? thumbNailUrl)
        {
            menuItem.AddThumbNailUrl(thumbNailUrl);
            await _menuItemRepository.UpdateMenuItemAsync(menuItem);
            return menuItem;
        }

        public async Task<MenuItem> SetMenuItemThumbNailUrlAsync(MenuItem menuItem, List<string>? picturesUrl, int index)
        {
            menuItem.SetThumbNail(picturesUrl, index);
            await _menuItemRepository.UpdateMenuItemAsync(menuItem);
            return menuItem;
        }

        public async Task<MenuItem> UpdateMenuItemThumbNailUrlAsync(MenuItem menuItem, string? thumbNailUrl)
        {
            menuItem.UpdateThumbNailUrl(thumbNailUrl);
            await _menuItemRepository.UpdateMenuItemAsync(menuItem);
            return menuItem;
        }

        public async Task<MenuItem> DeleteMenuItemThumbNailUrlAsync(MenuItem menuItem)
        {
            menuItem.DeleteThumbNailUrl();
            await _menuItemRepository.UpdateMenuItemAsync(menuItem);
            return menuItem;
        }

        public async Task<MenuItem> AddMenuItemPicturesUrlAsync(MenuItem menuItem, List<string>? picturesUrl)
        {
            menuItem.AddPicturesUrl(picturesUrl);
            await _menuItemRepository.UpdateMenuItemAsync(menuItem);
            return menuItem;
        }

        public async Task<MenuItem> UpdateMenuItemPicturesUrlAsync(MenuItem menuItem, string pUrl, int? i = null)
        {
            menuItem.UpdatePicturesUrl(pUrl, i);
            await _menuItemRepository.UpdateMenuItemAsync(menuItem);
            return menuItem;
        }

        public async Task<MenuItem> DeleteMenuItemPicturesUrlAsync(MenuItem menuItem, int index)
        {
            menuItem.DeletePicturesUrl(index);
            await _menuItemRepository.UpdateMenuItemAsync(menuItem);
            return menuItem;
        }

        public async Task<MenuItem> UpdateMenuItemCategoryAsync(MenuItem menuItem, string category)
        {
            menuItem.UpdateCategory(category);
            await _menuItemRepository.UpdateMenuItemAsync(menuItem);
            return menuItem;
        }

        public async Task SoftDeleteMenuItemAsync(MenuItem menuItem)
        {
            menuItem.SoftDelete();
            await _menuItemRepository.UpdateMenuItemAsync(menuItem);
        }


        public async Task<MenuItem?> GetMenuItemByIdAsync(Guid id, bool isDeleted = false, bool isActive = true)
        {
            MenuItem? menuItem = await _menuItemRepository.GetMenuItemByIdAsync(id);

            if (menuItem == null)
            {
                return null;
            }
            if (!isDeleted && menuItem.IsDeleted)
            {
                return null;
            }
            if (isActive && !menuItem.IsActive)
            {
                return null;
            }

            return menuItem;
        }

        public async Task<IReadOnlyList<MenuItem>> GetMenuItemsByNameAsync(string name)
        {
            return await _menuItemRepository.GetMenuItemByNameAsync(name);
        }

        public async Task<IReadOnlyList<MenuItem>> GetMenuItemsByCategoryAsync(string category)
        {
            return await _menuItemRepository.GetMenuItemsByCategoryAsync(category);
        }

        public async Task<IReadOnlyList<MenuItem>> GetAllMenuItemsAsync(bool isDeleted = false)
        {
            if (isDeleted)
            {
                return await _menuItemRepository.GetActiveMenuItemsAsync();
            }
            else
            {
                return await _menuItemRepository.GetAllMenuItemsAsync();
            }
        }

        public async Task<IReadOnlyList<MenuItem>> GetAllAvailableMenuItemsAsync()
        {
            return await _menuItemRepository.GetAvailableMenuItemsAsync();
        }
    }
}
