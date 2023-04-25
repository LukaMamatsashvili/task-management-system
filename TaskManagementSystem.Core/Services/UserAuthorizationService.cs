using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.DTOs;
using TaskManagementSystem.Core.Interfaces;
using TaskManagementSystem.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;
using TaskManagementSystem.Core.DataAccess;
using TaskManagementSystem.Core.Common;

namespace TaskManagementSystem.Core.Services
{
    public class UserAuthorizationService : IUserAuthorizationService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleService _userRoleService;
        private readonly HttpClient _httpClient;

        public UserAuthorizationService(IConfiguration configuration, IUserService userService, IUserRepository userRepository, 
                                        IUserRoleService userRoleService, HttpClient httpClient)
        {
            _configuration = configuration;
            _userService = userService;
            _userRepository = userRepository;
            _userRoleService = userRoleService;
            _httpClient = httpClient;
        }

        public async Task<string> RegisterUser(UserDTO UserDTO)
        {
            var result = await _userService.AddUser(UserDTO);

            return result;
        }

        public async Task<string> AuthorizeUser(UserDTO UserDTO)
        {
            if (UserDTO == null)
                throw new NotFoundException("User is null.");

            if (UserDTO.Username == null)
                throw new AppException("Username is required!");

            if (UserDTO.Password == null)
                throw new AppException("Password is required!");

            var token = await AuthenticateUser(UserDTO.Username, UserDTO.Password);

            if (string.IsNullOrEmpty(token))
                throw new AppException("Token is empty");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Add("token", token);

            var result = "Bearer " + token;

            return result;
        }

        private async Task<string> AuthenticateUser(string username, string password)
        {
            var User = await _userRepository.GetUserByUsernameAsync(username);

            if (User == null || !VerifyPasswordHash(password, User.PasswordHash, User.PasswordSalt))
            {
                return "Invalid credentials";
            }

            var userRole = (await _userRoleService.GetUserRoleById(User.UserRoleId)).Type;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretToken"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, User.Username),
                new Claim(ClaimTypes.Role, userRole)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
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
