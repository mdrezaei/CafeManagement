using MenuAndOrdering.API.Application.DTOs;
using MenuAndOrdering.API.Application.Services;
using MenuAndOrdering.API.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MenuAndOrdering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;

        public MenuController(IMenuItemService menuItemService) 
        {
            _menuItemService = menuItemService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<MenuItemDto>>> GetAll([FromQuery] int? tableId = null)
        {
            IReadOnlyList<MenuItem> menuItems = await _menuItemService.GetAllAvailableMenuItemsAsync();

            IReadOnlyList<MenuItemDto> result = menuItems.Select(m => new MenuItemDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                Category = m.Category,
                IsAvailable = m.IsAvailable,
                ThumbnailUrl = m.ThumbNailUrl,
                PicturesUrl = m.PicturesUrl
            }).ToList();

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MenuItemDto>> GetById(Guid id)
        {
            MenuItem? menuItem = await _menuItemService.GetMenuItemByIdAsync(id);

            if (menuItem == null)
            {
                return NotFound();
            }

            MenuItemDto result = new MenuItemDto
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                Category = menuItem.Category,
                IsAvailable = menuItem.IsAvailable,
                ThumbnailUrl = menuItem.ThumbNailUrl,
                PicturesUrl = menuItem.PicturesUrl
            };

            return Ok(result);

        }



    }
}
