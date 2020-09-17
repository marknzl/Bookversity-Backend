using Bookversity.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookversity.Api.Repositories
{
    public interface IOrdersRepository
    {
        List<Order> MyOrders(string userId);
        Order ViewOrder(int id, string userId);
    }
}
