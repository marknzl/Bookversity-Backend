using Bookversity.Api.Models;
using Bookversity.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bookversity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ExtendedUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly ImageStoreService _imageStoreService;
        private readonly AppDbContext _appDbContext;

        public UserController(UserManager<ExtendedUser> userManager, IOptionsSnapshot<JwtSettings> jwtSettings, ImageStoreService imageStoreService, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _imageStoreService = imageStoreService;
            _appDbContext = appDbContext;
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
                    await _imageStoreService.CreateUserContainer(user.Id);
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
                    var loginResponse = new LoginResponse
                    {
                        UserId = user.Id,
                        JwtToken = JwtService.GenerateJwtToken(user, _jwtSettings)
                    };

                    //return Ok(JwtService.GenerateJwtToken(user, _jwtSettings));
                    return Ok(loginResponse);
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

        [Authorize]
        [HttpGet("Overview")]
        public async Task<IActionResult> Overview()
        {
            string email = User.FindFirstValue(ClaimTypes.Name);

            var user = await _userManager.FindByNameAsync(email);
            var items = _appDbContext.Items.Where(i => i.SellerId == user.Id);

            int itemsListed = items.Count();
            int itemsForSale = 0;
            int itemsSold = 0;
            decimal totalDollarSales = 0;

            items.Where(i => !i.Sold).ToList().ForEach(i => itemsForSale++);
            items.Where(i => i.Sold).ToList().ForEach(i => itemsSold++);
            items.Where(i => i.Sold).ToList().ForEach(i => totalDollarSales += i.Price);

            var overviewModel = new OverviewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = email,
                ItemsListed = itemsListed,
                ItemsForSale = itemsForSale,
                TotalItemsSold = itemsSold,
                TotalDollarSales = totalDollarSales
            };

            return Ok(overviewModel);
        }
    }
}
