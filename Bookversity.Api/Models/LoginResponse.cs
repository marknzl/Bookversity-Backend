using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookversity.Api.Models
{
    public class LoginResponse
    {
        public string UserId { get; set; }
        public string JwtToken { get; set; }
    }
}
