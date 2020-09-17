using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Bookversity.Api.Models
{
    public class ItemPurchase
    {
        [Key]
        [JsonProperty("id")]
        public int Id { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
