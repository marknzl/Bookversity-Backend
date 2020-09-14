using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bookversity.Api.Models
{
    public class ItemPurchase
    {
        [Key]
        [JsonProperty("id")]
        public int Id;

        [Required]
        public int OrderId;
    }
}
