using Bookversity.Api.Models;
using Bookversity.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bookversity.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ExtendedUser> _userManager;
        //private readonly SignInManager<ExtendedUser> _signInManager;
        private readonly JwtSettings _jwtSettings;

        public UserController(UserManager<ExtendedUser> userManager, /*SignInManager<ExtendedUser> signInManager,*/ IOptionsSnapshot<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            //_signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ExtendedUser
                {
                    FirstName = registerModel.FirstName,
                    LastName = registerModel.Lastname,
                    UserName = registerModel.Email,
                    Email = registerModel.Email
                };

                var result = await _userManager.CreateAsync(user, registerModel.Password);

                if (result.Succeeded)
                {
                    return Ok();
                }
            }

            return BadRequest();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var user = _userManager.Users.SingleOrDefault(user => user.UserName == loginModel.Email);
            if (user == null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var correctPassword = await _userManager.CheckPasswordAsync(user, loginModel.Password);

                if (correctPassword)
                {
                    return Ok(JwtService.GenerateJwtToken(user, _jwtSettings));
                }
            }

            return BadRequest();
        }

        [Authorize]
        [HttpGet("Test")]
        public async Task<IActionResult> Test()
        {
            var email = User.FindFirstValue(ClaimTypes.Name);
            var user = await _userManager.FindByNameAsync(email);
            return Ok(new { user.FirstName, user.LastName, user.Id });
        }
    }
}
