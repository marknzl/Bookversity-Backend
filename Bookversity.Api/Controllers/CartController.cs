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
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public CartController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddToCart(int itemId)
        {
            var item = await _appDbContext.Items.FindAsync(itemId);

            if (item == null)
            {
                return BadRequest();
            }

            string userId = User.FindFirstValue("Id");

            if (userId == item.SellerId)
            {
                return BadRequest();
            }

            item.InCart = true;
            item.InUserCart = userId;
            _appDbContext.Entry(item).State = EntityState.Modified;
            await _appDbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Remove")]
        public async Task<IActionResult> RemoveFromCart(int itemId)
        {
            var item = await _appDbContext.Items.FindAsync(itemId);

            if (item == null)
            {
                return BadRequest();
            }

            item.InCart = false;
            item.InUserCart = "";
            _appDbContext.Entry(item).State = EntityState.Modified;
            await _appDbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("MyCart")]
        public IActionResult MyCart()
        {
            string userId = User.FindFirstValue("Id");

            var itemsInCart = _appDbContext.Items.Where(i => i.InUserCart == userId);

            return Ok(itemsInCart);
        }
    }
}
