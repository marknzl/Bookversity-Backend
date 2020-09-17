using Bookversity.Api.Models;
using Bookversity.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddToCart(int itemId)
        {
            string userId = User.FindFirstValue("Id");
            var item = await _cartRepository.AddToCart(itemId, userId);

            if (item == null)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("Remove")]
        public async Task<IActionResult> RemoveFromCart(int itemId)
        {
            string userId = User.FindFirstValue("Id");
            var item = await _cartRepository.RemoveFromCart(itemId, userId);

            if (item == null)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("MyCart")]
        public IActionResult MyCart()
        {
            string userId = User.FindFirstValue("Id");
            var itemsInCart = _cartRepository.MyCart(userId);

            return Ok(itemsInCart);
        }

        [HttpPost("Checkout")]
        public async Task<IActionResult> Checkout()
        {
            string userId = User.FindFirstValue("Id");
            var order = await _cartRepository.Checkout(userId);

            return Ok(new { orderId = order.Id });
        }
    }
}
