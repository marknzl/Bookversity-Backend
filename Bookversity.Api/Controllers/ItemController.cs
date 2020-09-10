using Bookversity.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                Sold = false,
                InCart = false
            };

            _appDbContext.Items.Add(item);
            await _appDbContext.SaveChangesAsync();

            return Ok(item);
        }
    }
}
