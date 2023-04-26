using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using TaskManagementSystem.Core.DataAccess;
using TaskManagementSystem.Core.DTOs;
using TaskManagementSystem.Core.Interfaces;
using TaskManagementSystem.Core.Common;
using TaskManagementSystem.Infrastructure.Models;

namespace TaskManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class UserAuthorizationController : ControllerBase
    {

        private readonly IUserAuthorizationService _userAuthorizationService;

        public UserAuthorizationController(IUserAuthorizationService userAuthorizationService)
        {
            _userAuthorizationService = userAuthorizationService;
        }


        [HttpPost("register")]
        public async Task<Response> RegisterUser(UserDTO UserDTO)
        {
            var response = await _userAuthorizationService.RegisterUser(UserDTO);

            return response;
        }

        [HttpPost("login")]
        public async Task<TokenResponse> LoginUser(UserDTO UserDTO)
        {
            var response = await _userAuthorizationService.AuthorizeUser(UserDTO);

            return response;
        }
    }
}
