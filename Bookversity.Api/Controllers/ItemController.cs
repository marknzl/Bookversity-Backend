using Bookversity.Api.Models;
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
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public ItemController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateItem(NewItemModel newItemModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var item = new Item
            {
                SellerId = User.FindFirstValue("Id"),
                ItemName = newItemModel.ItemName,
                ItemDescription = newItemModel.ItemDescription,
                Price = newItemModel.ItemPrice,
                TimeCreated = DateTime.Now,
                Sold = false,
                InCart = false
            };

            _appDbContext.Items.Add(item);
            await _appDbContext.SaveChangesAsync();

            return Ok(item);
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
            var items = _appDbContext.Items.OrderByDescending(i => i.Id).Where(i => !i.Sold).Take(10);
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

            _appDbContext.Items.Remove(item);
            await _appDbContext.SaveChangesAsync();

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("Search")]
        public IActionResult Search(string itemName)
        {
            var items = _appDbContext.Items.Where(i => i.ItemName.ToLower().Contains(itemName.ToLower()) 
                                                    && !i.InCart 
                                                    && !i.Sold);
            return Ok(items);
        }
    }
}
