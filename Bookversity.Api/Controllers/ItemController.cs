using Bookversity.Api.Models;
using Bookversity.Api.Services;
using Microsoft.AspNetCore.Authorization;
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
        private readonly AppDbContext _appDbContext;
        private readonly ImageStoreService _imageStoreService;

        public ItemController(AppDbContext appDbContext, ImageStoreService imageStoreService)
        {
            _appDbContext = appDbContext;
            _imageStoreService = imageStoreService;
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
            var imageUploadResponse = await _imageStoreService.UploadImage(userId, newItemModel.Image);

            var item = new Item
            {
                SellerId = User.FindFirstValue("Id"),
                ItemName = newItemModel.ItemName,
                ItemDescription = newItemModel.ItemDescription,
                Price = newItemModel.ItemPrice,
                TimeCreated = DateTime.Now,
                ItemImageUrl = imageUploadResponse.ImageUrl,
                ImageFileName = imageUploadResponse.ImageFileName,
                InUserCart = "",
                Sold = false,
                InCart = false
            };

            _appDbContext.Items.Add(item);
            await _appDbContext.SaveChangesAsync();

            return Ok(item);
        }

        [AllowAnonymous]
        [HttpGet("GetItem")]
        public async Task<IActionResult> GetItem(int id)
        {
            var item = await _appDbContext.Items.FindAsync(id);

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
            var items = _appDbContext.Items.Where(i => i.SellerId == id);

            return Ok(items);
        }

        [AllowAnonymous]
        [HttpGet("Latest10")]
        public IActionResult Latest10()
        {
            var items = _appDbContext.Items.OrderByDescending(i => i.Id).Where(i => !i.Sold && !i.InCart);
            return Ok(items);
        }

        [HttpDelete("DeleteItem")]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            var item = await _appDbContext.Items.FindAsync(itemId);

            if (item == null)
            {
                return BadRequest();
            }

            if (User.FindFirstValue("Id") != item.SellerId)
                return BadRequest();

            Console.WriteLine();
            await _imageStoreService.DeleteImage(item);
            _appDbContext.Items.Remove(item);
            await _appDbContext.SaveChangesAsync();

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

            var items = _appDbContext.Items.Where(i => i.ItemName.ToLower().Contains(itemName.ToLower()) 
                                                    && !i.InCart 
                                                    && !i.Sold);
            return Ok(items);
        }
    }
}
