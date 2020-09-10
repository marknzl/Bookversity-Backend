using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookversity.Api.Models
{
    public class UserCartModel
    {
        public Dictionary<string, Item> ItemsInCart;

        public UserCartModel()
        {
            ItemsInCart = new Dictionary<string, Item>();
        }
    }
}
