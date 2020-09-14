using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookversity.Api.Models
{
    public class OverviewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int ItemsListed { get; set; }
        public int ItemsForSale { get; set; }
        public int TotalItemsSold { get; set; }
        public decimal TotalDollarSales { get; set; }
    }
}
