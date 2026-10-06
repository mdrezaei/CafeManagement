using CafeManagement.API.Domain.Entities;
using CafeManagement.API.Domain.Interfaces;
using CafeManagement.Shared.Events;

namespace CafeManagement.API.Application.Services
{
    public class MMenuItemService : IMMenuItemService
    {
        private readonly IMMenuItemRepository _mMenuItemRepository;
        private readonly IOutboxService _outboxService;
        public MMenuItemService(IMMenuItemRepository mMenuItemRepository, IOutboxService outboxService)
        {
            _mMenuItemRepository = mMenuItemRepository;
            _outboxService = outboxService;
        }

        public async Task<MMenuItem> CreateMenuItemAsync(string name, decimal price, string category)
        {
            MMenuItem menuItem = new MMenuItem(name, price, category);
            await _mMenuItemRepository.AddMenuItemAsync(menuItem);

            await _outboxService.AddMessageAsync(new MenuItemCreatedEvent
            {
                MenuItemId = menuItem.Id,
                Name = menuItem.Name,
                Price = menuItem.Price,
                Category = menuItem.Category,
                CreatedAt = DateTime.UtcNow
            });

            return menuItem;
        }

        public async Task<MMenuItem> UpdateMenuItemAsync(MMenuItem menuItem)
        {
            await _mMenuItemRepository.UpdateMenuItemAsync(menuItem);

            await PublishMenuItemUpdated(menuItem);

            return menuItem;
        }

        public async Task<MMenuItem> SetAvailableAsync(MMenuItem menuItem)
        {
            menuItem.SetAvailable();
            await _mMenuItemRepository.UpdateMenuItemAsync(menuItem);

            await PublishMenuItemUpdated(menuItem);

            return menuItem;
        }

        public async Task<MMenuItem> SetUnavailableAsync(MMenuItem menuItem)
        {
            menuItem.SetUnavailable();
            await _mMenuItemRepository.UpdateMenuItemAsync(menuItem);

            await PublishMenuItemUpdated(menuItem);

            return menuItem;
        }

        public async Task<MMenuItem> ActivateAsync(MMenuItem menuItem)
        {
            menuItem.Activate();
            await _mMenuItemRepository.UpdateMenuItemAsync(menuItem);

            await PublishMenuItemUpdated(menuItem);

            return menuItem;
        }

        public async Task<MMenuItem> DeactivateAsync(MMenuItem menuItem)
        {
            menuItem.Deactivate();
            await _mMenuItemRepository.UpdateMenuItemAsync(menuItem);

            await PublishMenuItemUpdated(menuItem);

            return menuItem;
        }

        public async Task<MMenuItem> UpdateMenuItemNameAsync(MMenuItem menuItem, string name)
        {
            menuItem.UpdateName(name);
            await _mMenuItemRepository.UpdateMenuItemAsync(menuItem);

            await PublishMenuItemUpdated(menuItem);

            return menuItem;
        }

        public async Task<MMenuItem> UpdateMenuItemPriceAsync(MMenuItem menuItem, decimal price)
        {
            menuItem.UpdatePrice(price);
            await _mMenuItemRepository.UpdateMenuItemAsync(menuItem);

            await PublishMenuItemUpdated(menuItem);

            return menuItem;
        }

        public async Task<MMenuItem> UpdateMenuItemDescriptionAsync(MMenuItem menuItem, string? description)
        {
            menuItem.UpdateDescription(description);
            await _mMenuItemRepository.UpdateMenuItemAsync(menuItem);

            await PublishMenuItemUpdated(menuItem);

            return menuItem;
        }

        public async Task<MMenuItem> DeleteMenuItemDescriptionAsync(MMenuItem menuItem)
        {
            menuItem.DeleteDescription();
            await _mMenuItemRepository.UpdateMenuItemAsync(menuItem);

            await PublishMenuItemUpdated(menuItem);

            return menuItem;
        }

        public async Task<MMenuItem> AddMenuItemThumbNailUrlAsync(MMenuItem menuItem, string? thumbNailUrl)
        {
            menuItem.AddThumbNailUrl(thumbNailUrl);
            await _mMenuItemRepository.UpdateMenuItemAsync(menuItem);

            await PublishMenuItemUpdated(menuItem);

            return menuItem;
        }

        public async Task<MMenuItem> SetMenuItemThumbNailUrlAsync(MMenuItem menuItem, List<string>? picturesUrl, int index)
        {
            menuItem.SetThumbNail(picturesUrl, index);
            await _mMenuItemRepository.UpdateMenuItemAsync(menuItem);

            await PublishMenuItemUpdated(menuItem);

            return menuItem;
        }

        public async Task<MMenuItem> UpdateMenuItemThumbNailUrlAsync(MMenuItem menuItem, string? thumbNailUrl)
        {
            menuItem.UpdateThumbNailUrl(thumbNailUrl);
            await _mMenuItemRepository.UpdateMenuItemAsync(menuItem);

            await PublishMenuItemUpdated(menuItem);

            return menuItem;
        }

        public async Task<MMenuItem> DeleteMenuItemThumbNailUrlAsync(MMenuItem menuItem)
        {
            menuItem.DeleteThumbNailUrl();
            await _mMenuItemRepository.UpdateMenuItemAsync(menuItem);

            await PublishMenuItemUpdated(menuItem);

            return menuItem;
        }

        public async Task<MMenuItem> AddMenuItemPicturesUrlAsync(MMenuItem menuItem, List<string>? picturesUrl)
        {
            menuItem.AddPicturesUrl(picturesUrl);
            await _mMenuItemRepository.UpdateMenuItemAsync(menuItem);

            await PublishMenuItemUpdated(menuItem);

            return menuItem;
        }

        public async Task<MMenuItem> UpdateMenuItemPicturesUrlAsync(MMenuItem menuItem, string pUrl, int? i = null)
        {
            menuItem.UpdatePicturesUrl(pUrl, i);
            await _mMenuItemRepository.UpdateMenuItemAsync(menuItem);

            await PublishMenuItemUpdated(menuItem);

            return menuItem;
        }

        public async Task<MMenuItem> DeleteMenuItemPicturesUrlAsync(MMenuItem menuItem, int index)
        {
            menuItem.DeletePicturesUrl(index);
            await _mMenuItemRepository.UpdateMenuItemAsync(menuItem);

            await PublishMenuItemUpdated(menuItem);

            return menuItem;
        }

        public async Task<MMenuItem> UpdateMenuItemCategoryAsync(MMenuItem menuItem, string category)
        {
            menuItem.UpdateCategory(category);
            await _mMenuItemRepository.UpdateMenuItemAsync(menuItem);

            await PublishMenuItemUpdated(menuItem);

            return menuItem;
        }

        public async Task SoftDeleteMenuItemAsync(MMenuItem menuItem)
        {
            menuItem.SoftDelete();
            await _mMenuItemRepository.UpdateMenuItemAsync(menuItem);

            await _outboxService.AddMessageAsync(new MenuItemDeletedEvent
            {
                MenuItemId = menuItem.Id,
                DeletedAt = DateTime.UtcNow
            });

        }

        public async Task<IReadOnlyList<MMenuItem>> GetAllMenuItemsAsync(bool includeDeleted = false)
        {
            return await _mMenuItemRepository.GetAllMenuItemsAsync(includeDeleted);
        }

        public async Task<MMenuItem?> GetMenuItemByIdAsync(Guid id, bool includeDeleted = false)
        {
            return await _mMenuItemRepository.GetMenuItemByIdAsync(id, includeDeleted);
        }

        public async Task<IReadOnlyList<MMenuItem>> GetMenuItemByNameAsync(string name, bool includeDeleted = false)
        {
            return await _mMenuItemRepository.GetMenuItemByNameAsync(name, includeDeleted);
        }

        public async Task<IReadOnlyList<MMenuItem>> GetMenuItemsByCategoryAsync(string category, bool includeDeleted = false)
        {
            return await _mMenuItemRepository.GetMenuItemsByCategoryAsync(category, includeDeleted);
        }

        public async Task<IReadOnlyList<MMenuItem>> GetAvailableMenuItemsAsync(bool includeDeleted = false)
        {
            return await _mMenuItemRepository.GetAvailableMenuItemsAsync(includeDeleted);
        }

        public async Task<IReadOnlyList<MMenuItem>> GetActiveMenuItemsAsync(bool includeDeleted = false)
        {
            return await _mMenuItemRepository.GetActiveMenuItemsAsync(includeDeleted);
        }

        private async Task PublishMenuItemUpdated(MMenuItem menuItem)
        {
            await _outboxService.AddMessageAsync(new MenuItemUpdatedEvent
            {
                MenuItemId = menuItem.Id,
                Name = menuItem.Name,
                Price = menuItem.Price,
                Category = menuItem.Category,
                UpdatedAt = DateTime.UtcNow
            });

        }
    }
}
