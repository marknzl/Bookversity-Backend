using Bookversity.Api.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookversity.Api.Repositories
{
    public interface IItemRepository
    {
        Task<Item> CreateItem(NewItemModel newItemModel, ExtendedUser user);
        Task<Item> GetItem(int id);
        IOrderedQueryable<Item> MyItems(string userId);
        IOrderedQueryable<Item> Latest();
        Task<Item> DeleteItem(int id, string userId);
        IOrderedQueryable<Item> Search(string itemName);
    }
}
