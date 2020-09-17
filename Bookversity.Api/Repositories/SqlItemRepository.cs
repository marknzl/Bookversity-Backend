using Bookversity.Api.Models;
using Bookversity.Api.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookversity.Api.Repositories
{
    public class SqlItemRepository : IItemRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly ImageStoreService _imageStoreService;

        public SqlItemRepository(AppDbContext appDbContext, ImageStoreService imageStoreService)
        {
            _appDbContext = appDbContext;
            _imageStoreService = imageStoreService;
        }

        public async Task<Item> CreateItem(NewItemModel newItemModel, ExtendedUser user)
        {
            var imageUploadResponse = await _imageStoreService.UploadImage(user.Id, newItemModel.Image);

            var item = new Item
            {
                SellerId = user.Id,
                SellerEmail = user.Email,
                ItemName = newItemModel.ItemName,
                ItemDescription = newItemModel.ItemDescription,
                Price = newItemModel.ItemPrice,
                TimeCreated = DateTime.Now,
                ItemImageUrl = imageUploadResponse.ImageUrl,
                ImageFileName = imageUploadResponse.ImageFileName,
                InUserCart = "",
                Sold = false,
                InCart = false
            };

            await _appDbContext.Items.AddAsync(item);
            await _appDbContext.SaveChangesAsync();

            return item;
        }

        public async Task<Item> DeleteItem(int id, string userId)
        {
            var item = await _appDbContext.Items.FindAsync(id);
            if (item != null)
            {
                if (item.InCart || item.Sold)
                {
                    return null;
                }
                else if (item.SellerId != userId)
                {
                    return null;
                }

                _appDbContext.Remove(item);
                await _imageStoreService.DeleteImage(item);
                await _appDbContext.SaveChangesAsync();
            }
            else if (item == null)
            {
                return null;
            }

            return item;
        }

        public async Task<Item> GetItem(int id)
        {
            return await _appDbContext.Items.FindAsync(id);
        }

        public IOrderedQueryable<Item> Latest()
        {
            return _appDbContext.Items.Where(i => !i.Sold && !i.InCart).OrderByDescending(i => i.Id);
        }

        public IOrderedQueryable<Item> MyItems(string userId)
        {
            return _appDbContext.Items.Where(i => i.SellerId == userId).OrderByDescending(i => i.Id);
        }

        public IOrderedQueryable<Item> Search(string itemName)
        {
            return _appDbContext.Items.Where(i => i.ItemName.ToLower().Contains(itemName.ToLower())
                                             && !i.InCart
                                             && !i.Sold).OrderByDescending(i => i.Id);
        }
    }
}
