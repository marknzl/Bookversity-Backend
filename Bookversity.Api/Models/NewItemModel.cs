using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Bookversity.Api.Models
{
    public class NewItemModel
    {
        [Required]
        public string ItemName { get; set; }

        [Required]
        public string ItemDescription { get; set; }

        [Required]
        public decimal ItemPrice { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
}
