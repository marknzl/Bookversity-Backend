using Bookversity.Api.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bookversity.Api.Services
{
    public class JwtService
    {
        private JwtService()
        {

        }

        private static JwtService _instance = null;

        public static JwtService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new JwtService();
                }

                return _instance;
            }
        }

        public string GenerateJwtToken(ExtendedUser user, JwtSettings jwtSettings)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),

                Expires = DateTime.Now.AddDays(jwtSettings.ExpirationInDays),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
                Audience = jwtSettings.Issuer,
                Issuer = jwtSettings.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
