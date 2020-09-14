using Bookversity.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var orders = _appDbContext.Orders.Where(o => o.UserId == userId);

            return Ok(orders);
        }
    }
}
