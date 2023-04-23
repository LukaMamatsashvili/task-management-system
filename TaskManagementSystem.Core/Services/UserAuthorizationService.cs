using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Interfaces;
using TaskManagementSystem.Infrastructure.Models;

namespace TaskManagementSystem.Core.Services
{
    public class UserAuthorizationService : IUserAuthorizationService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public UserAuthorizationService(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        public async Task<string> AuthenticateAsync(string username, string password)
        {
            var user = new User(); //droebit mivawere es rom errori ar iyos, mere washale da es datove -> //await _userService.GetUserByUserNameAsync(username);

            if (user == null || !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return "Invalid credentials";
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Username.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != passwordHash[i]) return false;
            }
            return true;
        }
    }
}
