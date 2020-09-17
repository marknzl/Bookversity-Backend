using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookversity.Api.Models
{
    public class Item
    {
        [Key]
        [JsonProperty("id")]
        public int Id { get; set; }

        [Required]
        public string SellerId { get; set; }

        [Required]
        public string SellerEmail { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public string ItemDescription { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public DateTime TimeCreated { get; set; }

        public string ItemImageUrl { get; set; }
        public string ImageFileName { get; set; }
        public string InUserCart { get; set; }

        public bool Sold { get; set; }
        public bool InCart { get; set; }

        [JsonIgnore]
        public ItemPurchase ItemPurchase { get; set; }
    }
}
