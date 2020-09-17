using Bookversity.Api.Models;
using Bookversity.Api.Repositories;
using Bookversity.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bookversity.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly UserManager<ExtendedUser> _userManager;
        private readonly IItemRepository _itemRepository;

        public ItemController(UserManager<ExtendedUser> userManager, IItemRepository itemRepository)
        {
            _userManager = userManager;
            _itemRepository = itemRepository;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateItem([FromForm] NewItemModel newItemModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!(newItemModel.Image.Length > 0))
            {
                return BadRequest();
            }

            string userId = User.FindFirstValue("Id");
            var user = await _userManager.FindByIdAsync(userId);

            var item = await _itemRepository.CreateItem(newItemModel, user);

            return Ok(item);
        }

        [AllowAnonymous]
        [HttpGet("GetItem")]
        public async Task<IActionResult> GetItem(int id)
        {
            var item = await _itemRepository.GetItem(id);

            if (item == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(item);
            }
        }

        [HttpGet("MyItems")]
        public IActionResult MyItems()
        {
            string id = User.FindFirstValue("Id");
            var items = _itemRepository.MyItems(id);

            return Ok(items);
        }

        [AllowAnonymous]
        [HttpGet("Latest10")]
        public IActionResult Latest10()
        {
            var items = _itemRepository.Latest10();
            return Ok(items);
        }

        [HttpDelete("DeleteItem")]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            string userId = User.FindFirstValue("Id");
            var item = await _itemRepository.DeleteItem(itemId, userId);

            if (item == null)
            {
                return BadRequest();
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("Search")]
        public IActionResult Search(string itemName)
        {
            if (itemName == null)
            {
                itemName = "";
            }

            var items = _itemRepository.Search(itemName);
            return Ok(items);
        }
    }
}
