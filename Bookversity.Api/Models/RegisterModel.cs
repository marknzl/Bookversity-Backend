using System.ComponentModel.DataAnnotations;

namespace Bookversity.Api.Models
{
    public class RegisterModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
