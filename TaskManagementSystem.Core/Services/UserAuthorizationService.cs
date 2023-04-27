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
using System.Net;

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

        public async Task<Response> RegisterUser(UserDTO UserDTO)
        {
            var response = await _userService.AddUser(UserDTO);

            if(response.ResponseMessage.StatusCode == HttpStatusCode.OK)
                response.Message = "Successful Registration!";

            return response;
        }

        public async Task<TokenResponse> AuthorizeUser(UserDTO UserDTO)
        {
            var response = new TokenResponse();
            try
            {
                if (UserDTO == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "User is null!";

                    return response;
                }

                if (UserDTO.Username == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Username is required!";

                    return response;
                }

                if (UserDTO.Password == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Password is required!";

                    return response;
                }

                var tokenResponse = await AuthenticateUser(UserDTO.Username, UserDTO.Password);

                if (tokenResponse.ResponseMessage.StatusCode != HttpStatusCode.OK)
                    return tokenResponse;

                if (string.IsNullOrEmpty(tokenResponse.Token))
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Token is Empty!";

                    return response;
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Token);

                response.Token = tokenResponse.Token;

                response.ResponseMessage.StatusCode = HttpStatusCode.OK;
                response.Message = "Successful Authorization!";

                return response;
            }
            catch (Exception ex)
            {
                response.ResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;

                return response;
            }
        }

        private async Task<TokenResponse> AuthenticateUser(string username, string password)
        {
            var response = new TokenResponse();

            var User = await _userRepository.GetUserByUsernameAsync(username);
            if (User == null || !VerifyPasswordHash(password, User.PasswordHash, User.PasswordSalt))
            {
                response.ResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Invalid Credentials!";

                return response;
            }

            var userRole = (await _userRoleService.GetUserRoleById(User.UserRoleId)).UserRole.Type;

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, userRole)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:SecretToken"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            response.Token = jwt;

            response.ResponseMessage.StatusCode = HttpStatusCode.OK;
            response.Message = "Token Created Successfully!";

            return response;
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
