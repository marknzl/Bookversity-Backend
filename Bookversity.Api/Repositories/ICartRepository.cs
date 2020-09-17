using Bookversity.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookversity.Api.Repositories
{
    public interface ICartRepository
    {
        Task<Item> AddToCart(int itemId, string userId);
        Task<Item> RemoveFromCart(int itemId, string userId);
        IQueryable<Item> MyCart(string userId);
        Task<Order> Checkout(string userId);
    }
}
