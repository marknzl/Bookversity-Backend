using Microsoft.AspNetCore.Identity;

namespace Bookversity.Api.Models
{
    public class ExtendedUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
