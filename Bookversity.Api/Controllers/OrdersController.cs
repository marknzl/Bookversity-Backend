using Bookversity.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bookversity.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersRepository _ordersRepository;

        public OrdersController(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        [HttpGet("MyOrders")]
        public IActionResult MyOrders()
        {
            string userId = User.FindFirstValue("Id");
            var orders = _ordersRepository.MyOrders(userId);

            return Ok(orders);
        }

        [HttpGet("ViewOrder")]
        public IActionResult ViewOrder(int orderId)
        {
            string userId = User.FindFirstValue("Id");
            var order = _ordersRepository.ViewOrder(orderId, userId);

            if (order == null)
            {
                return BadRequest();
            }

            return Ok(order);
        }
    }
}
