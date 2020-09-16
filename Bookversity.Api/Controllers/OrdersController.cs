using Bookversity.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bookversity.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public OrdersController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext; 
        }

        [HttpGet("MyOrders")]
        public IActionResult MyOrders()
        {
            string userId = User.FindFirstValue("Id");
            var orders = _appDbContext.Orders.Include(o => o.ItemsPurchased)
                .ThenInclude(ip => ip.Item).
                Where(o => o.UserId == userId).ToList();

            return Ok(orders);
        }

        [HttpGet("ViewOrder")]
        public IActionResult ViewOrder(int orderId)
        {
            string userId = User.FindFirstValue("Id");
            var order = _appDbContext.Orders.Include(o => o.ItemsPurchased)
                .ThenInclude(ip => ip.Item)
                .Where(o => o.UserId == userId && o.Id == orderId).First();

            if (order == null)
            {
                return BadRequest();
            }

            if (order.UserId != userId)
            {
                return BadRequest();
            }

            return Ok(order);
        }
    }
}
