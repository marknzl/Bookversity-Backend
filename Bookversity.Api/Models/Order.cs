using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookversity.Api.Models
{
    public class Order
    {
        [Key]
        [JsonProperty("id")]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        public IList<ItemPurchase> ItemsPurchased { get; set; }
    }
}
