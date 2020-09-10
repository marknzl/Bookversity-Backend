using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bookversity.Api.Models;
using Bookversity.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Bookversity.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private UserManager<ExtendedUser> _userManager;
        private SignInManager<ExtendedUser> _signInManager;
        private JwtSettings _jwtSettings;

        public UserController(UserManager<ExtendedUser> userManager, SignInManager<ExtendedUser> signInManager, IOptionsSnapshot<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                    var jwtService = JwtService.Instance;
                    return Ok(jwtService.GenerateJwtToken(user, _jwtSettings));
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

        [HttpGet("NewFeatureTest")]
        public IActionResult NewFeatureTest()
        {
            return Ok();
        }
    }
}
