using CafeManagement.API.Application.DTOs;
using CafeManagement.API.Application.DTOs.MenuPictures;
using CafeManagement.API.Application.Services;
using CafeManagement.API.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CafeManagement.API.Controllers
{
    [Authorize]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class MMenuItemController : ControllerBase
    {
        private readonly IMMenuItemService _mMenuItemService;

        public MMenuItemController(IMMenuItemService menuItemService)
        {
            _mMenuItemService = menuItemService;
        }


        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<MMenuItemDto>>> GetAllMenuItem([FromQuery] bool includeDeleted = false, [FromQuery] bool onlyActive = true)
        {
            IReadOnlyList<MMenuItem> menuItems;

            if (onlyActive)
            {
                menuItems = await _mMenuItemService.GetActiveMenuItemsAsync(includeDeleted);
            }
            else
            {
                menuItems = await _mMenuItemService.GetAllMenuItemsAsync(includeDeleted);
            }

            IReadOnlyList<MMenuItemDto> result = menuItems.Select(m => new MMenuItemDto()
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                IsAvailable = m.IsAvailable,
                IsActive = m.IsActive,
                ThumbNailUrl = m.ThumbNailUrl,
                PicturesUrl = m.PicturesUrl,
                Category = m.Category
            }).ToList();

            return Ok(result);

        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MMenuItemDto>> GetMenuItemById(Guid id, [FromQuery] bool includeDeleted = false)
        {
            MMenuItem? menuItem = await _mMenuItemService.GetMenuItemByIdAsync(id, includeDeleted);

            if (menuItem == null)
            {
                return NotFound();
            }

            MMenuItemDto result = new MMenuItemDto()
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                IsAvailable = menuItem.IsAvailable,
                IsActive = menuItem.IsActive,
                ThumbNailUrl = menuItem.ThumbNailUrl,
                PicturesUrl = menuItem.PicturesUrl,
                Category = menuItem.Category
            };

            return Ok(result);

        }

        [HttpGet("by-name/{name}")]
        public async Task<ActionResult<IReadOnlyList<MMenuItemDto>>> GetMenuItemByName(string name, [FromQuery] bool includeDeleted = false)
        {
            IReadOnlyList<MMenuItem> menuItems = await _mMenuItemService.GetMenuItemByNameAsync(name, includeDeleted);

            IReadOnlyList<MMenuItemDto> result = menuItems.Select(m => new MMenuItemDto()
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                IsAvailable = m.IsAvailable,
                IsActive = m.IsActive,
                ThumbNailUrl = m.ThumbNailUrl,
                PicturesUrl = m.PicturesUrl,
                Category = m.Category
            }).ToList();

            return Ok(result);

        }

        [HttpGet("by-category/{category}")]
        public async Task<ActionResult<IReadOnlyList<MMenuItemDto>>> GetMenuItemByCategory(string category, [FromQuery] bool includeDeleted = false)
        {
            IReadOnlyList<MMenuItem> menuItems = await _mMenuItemService.GetMenuItemsByCategoryAsync(category, includeDeleted);

            IReadOnlyList<MMenuItemDto> result = menuItems.Select(m => new MMenuItemDto()
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                IsAvailable = m.IsAvailable,
                IsActive = m.IsActive,
                ThumbNailUrl = m.ThumbNailUrl,
                PicturesUrl = m.PicturesUrl,
                Category = m.Category
            }).ToList();

            return Ok(result);

        }

        [HttpGet("only-available")]
        public async Task<ActionResult<IReadOnlyList<MMenuItemDto>>> GetAvailableMenuItems([FromQuery] bool includeDeleted = false)
        {
            IReadOnlyList<MMenuItem> menuItems = await _mMenuItemService.GetAvailableMenuItemsAsync(includeDeleted);

            IReadOnlyList<MMenuItemDto> result = menuItems.Select(m => new MMenuItemDto()
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                IsAvailable = m.IsAvailable,
                IsActive = m.IsActive,
                ThumbNailUrl = m.ThumbNailUrl,
                PicturesUrl = m.PicturesUrl,
                Category = m.Category
            }).ToList();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateMenuItem([FromBody] CreateMenuItemRequest createMenuItemRequest)
        {
            MMenuItem menuItem = await _mMenuItemService.CreateMenuItemAsync(
                createMenuItemRequest.Name, createMenuItemRequest.Price, createMenuItemRequest.Category);

            if (!string.IsNullOrWhiteSpace(createMenuItemRequest.Description))
            {
                menuItem = await _mMenuItemService.UpdateMenuItemDescriptionAsync(menuItem, createMenuItemRequest.Description);
            }

            MMenuItemDto result = new MMenuItemDto()
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                IsAvailable = menuItem.IsAvailable,
                IsActive = menuItem.IsActive,
                ThumbNailUrl = menuItem.ThumbNailUrl,
                PicturesUrl = menuItem.PicturesUrl,
                Category = menuItem.Category
            };

            return CreatedAtAction(nameof(GetMenuItemById), new { id = result.Id }, result);

        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<MMenuItemDto>> PatchMenuItem(Guid id, [FromBody] UpdateMenuItemRequest request)
        {
            MMenuItem? menuItem = await _mMenuItemService.GetMenuItemByIdAsync(id);

            if (menuItem == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                menuItem = await _mMenuItemService.UpdateMenuItemNameAsync(menuItem, request.Name);
            }

            if (request.Price.HasValue)
            {
                menuItem = await _mMenuItemService.UpdateMenuItemPriceAsync(menuItem, request.Price.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Category))
            {
                menuItem = await _mMenuItemService.UpdateMenuItemCategoryAsync(menuItem, request.Category);
            }

            if (request.Description != null)
            {
                menuItem = await _mMenuItemService.UpdateMenuItemDescriptionAsync(menuItem, request.Description);
            }

            MMenuItemDto result = new MMenuItemDto()
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                IsAvailable = menuItem.IsAvailable,
                IsActive = menuItem.IsActive,
                ThumbNailUrl = menuItem.ThumbNailUrl,
                PicturesUrl = menuItem.PicturesUrl,
                Category = menuItem.Category
            };

            return Ok(result);
        }

        [HttpPut("{id:guid}/available")]
        public async Task<ActionResult<MMenuItemDto>> SetAvailable(Guid id)
        {
            MMenuItem? menuItem = await _mMenuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }

            menuItem = await _mMenuItemService.SetAvailableAsync(menuItem);

            MMenuItemDto result = new MMenuItemDto()
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                IsAvailable = menuItem.IsAvailable,
                IsActive = menuItem.IsActive,
                ThumbNailUrl = menuItem.ThumbNailUrl,
                PicturesUrl = menuItem.PicturesUrl,
                Category = menuItem.Category
            };

            return Ok(result);
        }

        [HttpPut("{id:guid}/unavailable")]
        public async Task<ActionResult<MMenuItemDto>> SetUnavailable(Guid id)
        {
            MMenuItem? menuItem = await _mMenuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }

            menuItem = await _mMenuItemService.SetUnavailableAsync(menuItem);

            MMenuItemDto result = new MMenuItemDto()
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                IsAvailable = menuItem.IsAvailable,
                IsActive = menuItem.IsActive,
                ThumbNailUrl = menuItem.ThumbNailUrl,
                PicturesUrl = menuItem.PicturesUrl,
                Category = menuItem.Category
            };

            return Ok(result);
        }

        [HttpPut("{id:guid}/activate")]
        public async Task<ActionResult<MMenuItemDto>> Activate(Guid id)
        {
            MMenuItem? menuItem = await _mMenuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            menuItem = await _mMenuItemService.ActivateAsync(menuItem);

            MMenuItemDto result = new MMenuItemDto()
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                IsAvailable = menuItem.IsAvailable,
                IsActive = menuItem.IsActive,
                ThumbNailUrl = menuItem.ThumbNailUrl,
                PicturesUrl = menuItem.PicturesUrl,
                Category = menuItem.Category
            };

            return Ok(result);
        }

        [HttpPut("{id:guid}/deactivate")]
        public async Task<ActionResult<MMenuItemDto>> Deactivate(Guid id)
        {
            MMenuItem? menuItem = await _mMenuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            menuItem = await _mMenuItemService.DeactivateAsync(menuItem);

            MMenuItemDto result = new MMenuItemDto()
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                IsAvailable = menuItem.IsAvailable,
                IsActive = menuItem.IsActive,
                ThumbNailUrl = menuItem.ThumbNailUrl,
                PicturesUrl = menuItem.PicturesUrl,
                Category = menuItem.Category
            };

            return Ok(result);
        }

        [HttpPost("{id:guid}/thumbnail")]
        public async Task<ActionResult<MMenuItemDto>> AddThumbnail(Guid id, [FromBody] ThumbnailRequest request)
        {
            MMenuItem? menuItem = await _mMenuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            menuItem = await _mMenuItemService.AddMenuItemThumbNailUrlAsync(menuItem, request.ThumbNailUrl);

            MMenuItemDto result = new MMenuItemDto()
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                IsAvailable = menuItem.IsAvailable,
                IsActive = menuItem.IsActive,
                ThumbNailUrl = menuItem.ThumbNailUrl,
                PicturesUrl = menuItem.PicturesUrl,
                Category = menuItem.Category
            };

            return Ok(result);
        }

        [HttpPost("{id:guid}/thumbnail/set")]
        public async Task<ActionResult<MMenuItemDto>> SetThumbnailFromPictures(Guid id, [FromBody] SetThumbnailRequest request)
        {
            MMenuItem? menuItem = await _mMenuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            menuItem = await _mMenuItemService.SetMenuItemThumbNailUrlAsync(menuItem, menuItem.PicturesUrl, request.Index);

            MMenuItemDto result = new MMenuItemDto()
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                IsAvailable = menuItem.IsAvailable,
                IsActive = menuItem.IsActive,
                ThumbNailUrl = menuItem.ThumbNailUrl,
                PicturesUrl = menuItem.PicturesUrl,
                Category = menuItem.Category
            };

            return Ok(result);
        }

        [HttpPut("{id:guid}/thumbnail")]
        public async Task<ActionResult<MMenuItemDto>> UpdateThumbnail(Guid id, [FromBody] ThumbnailRequest request)
        {
            MMenuItem? menuItem = await _mMenuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            menuItem = await _mMenuItemService.UpdateMenuItemThumbNailUrlAsync(menuItem, request.ThumbNailUrl);

            MMenuItemDto result = new MMenuItemDto()
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                IsAvailable = menuItem.IsAvailable,
                IsActive = menuItem.IsActive,
                ThumbNailUrl = menuItem.ThumbNailUrl,
                PicturesUrl = menuItem.PicturesUrl,
                Category = menuItem.Category
            };

            return Ok(result);
        }

        [HttpDelete("{id:guid}/thumbnail")]
        public async Task<ActionResult> DeleteThumbnail(Guid id)
        {
            MMenuItem? menuItem = await _mMenuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            menuItem = await _mMenuItemService.DeleteMenuItemThumbNailUrlAsync(menuItem);

            return NoContent();
        }


        [HttpPost("{id:guid}/pictures")]
        public async Task<ActionResult<MMenuItemDto>> AddPictures(Guid id, [FromBody] PicturesRequest request)
        {
            MMenuItem? menuItem = await _mMenuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            menuItem = await _mMenuItemService.AddMenuItemPicturesUrlAsync(menuItem, request.PicturesUrl);

            MMenuItemDto result = new MMenuItemDto()
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                IsAvailable = menuItem.IsAvailable,
                IsActive = menuItem.IsActive,
                ThumbNailUrl = menuItem.ThumbNailUrl,
                PicturesUrl = menuItem.PicturesUrl,
                Category = menuItem.Category
            };

            return Ok(result);
        }

        [HttpPut("{id:guid}/pictures/{index:int}")]
        public async Task<ActionResult<MMenuItemDto>> UpdatePicture(Guid id, int index, [FromBody] UpdatePictureRequest request)
        {
            MMenuItem? menuItem = await _mMenuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            menuItem = await _mMenuItemService.UpdateMenuItemPicturesUrlAsync(menuItem, request.Url, index);
            MMenuItemDto result = new MMenuItemDto()
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                IsAvailable = menuItem.IsAvailable,
                IsActive = menuItem.IsActive,
                ThumbNailUrl = menuItem.ThumbNailUrl,
                PicturesUrl = menuItem.PicturesUrl,
                Category = menuItem.Category
            };

            return Ok(result);
        }

        [HttpDelete("{id:guid}/pictures/{index:int}")]
        public async Task<ActionResult<MMenuItemDto>> DeletePicture(Guid id, int index)
        {
            MMenuItem? menuItem = await _mMenuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }

            menuItem = await _mMenuItemService.DeleteMenuItemPicturesUrlAsync(menuItem, index);
            return NoContent();
        }

        [HttpDelete("{id:guid}/description")]
        public async Task<ActionResult<MMenuItemDto>> DeleteDescription(Guid id)
        {
            MMenuItem? menuItem = await _mMenuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            menuItem = await _mMenuItemService.DeleteMenuItemDescriptionAsync(menuItem);
            return NoContent();
        }


        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> SoftDeleteMenuItem(Guid id)
        {
            MMenuItem? menuItem = await _mMenuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            await _mMenuItemService.SoftDeleteMenuItemAsync(menuItem);
            return NoContent();
        }
    }
}
