using Bookversity.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookversity.Api.Repositories
{
    public class SqlOrdersRepository : IOrdersRepository
    {
        private readonly AppDbContext _appDbContext;

        public SqlOrdersRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public List<Order> MyOrders(string userId)
        {
            var orders = _appDbContext.Orders.Include(o => o.ItemsPurchased)
                .ThenInclude(ip => ip.Item).
                Where(o => o.UserId == userId).ToList();

            return orders;
        }

        public Order ViewOrder(int id, string userId)
        {
            var order = _appDbContext.Orders.Include(o => o.ItemsPurchased)
                .ThenInclude(ip => ip.Item)
                .Where(o => o.UserId == userId && o.Id == id).First();

            if (order == null)
            {
                return null;
            }

            if (order.UserId != userId)
            {
                return null;
            }

            return order;
        }
    }
}
