using Bookversity.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookversity.Api.Repositories
{
    public class SqlCartRepository : ICartRepository
    {
        private readonly AppDbContext _appDbContext;

        public SqlCartRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Item> AddToCart(int itemId, string userId)
        {
            var item = await _appDbContext.Items.FindAsync(itemId);

            if (item == null)
            {
                return null;
            }

            if (userId == item.SellerId)
            {
                return null;
            }

            item.InCart = true;
            item.InUserCart = userId;
            _appDbContext.Entry(item).State = EntityState.Modified;
            await _appDbContext.SaveChangesAsync();

            return item;
        }

        public async Task<Order> Checkout(string userId)
        {
            var itemsInCart = _appDbContext.Items.Where(i => i.InUserCart == userId && !i.Sold).ToList();

            decimal total = 0;

            foreach (var item in itemsInCart)
            {
                item.InCart = false;
                item.InUserCart = "";
                item.Sold = true;

                total += item.Price;
                _appDbContext.Entry(item).State = EntityState.Modified;
            }

            var order = new Order
            {
                UserId = userId,
                Total = total,
                TransactionDate = DateTime.Now
            };

            await _appDbContext.Orders.AddAsync(order);
            await _appDbContext.SaveChangesAsync();

            foreach (var item in itemsInCart)
            {
                var itemPurchase = new ItemPurchase()
                {
                    ItemId = item.Id,
                    OrderId = order.Id
                };

                await _appDbContext.ItemPurchases.AddAsync(itemPurchase);
            }

            await _appDbContext.SaveChangesAsync();

            return order;
        }

        public IQueryable<Item> MyCart(string userId)
        {
            return _appDbContext.Items.Where(i => i.InUserCart == userId && !i.Sold);
        }

        public async Task<Item> RemoveFromCart(int itemId, string userId)
        {
            var item = await _appDbContext.Items.FindAsync(itemId);

            if (item == null)
            {
                return null;
            }

            if (item.InUserCart != userId)
            {
                return null;
            }

            item.InCart = false;
            item.InUserCart = "";
            _appDbContext.Entry(item).State = EntityState.Modified;
            await _appDbContext.SaveChangesAsync();

            return item;
        }
    }
}
