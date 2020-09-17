using System.Collections.Generic;

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
