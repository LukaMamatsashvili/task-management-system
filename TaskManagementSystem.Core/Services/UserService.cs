using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Common;
using TaskManagementSystem.Core.DataAccess;
using TaskManagementSystem.Core.DTOs;
using TaskManagementSystem.Core.Interfaces;
using TaskManagementSystem.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementSystem.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleService _userRoleService;

        public UserService(IUserRepository userRepository, IUserRoleService userRoleService)
        {
            _userRepository = userRepository;
            _userRoleService = userRoleService;
        }

        public async Task<UsersResponse> GetUsers()
        {
            var response = new UsersResponse();
            try
            {
                var users = await _userRepository.GetUsersAsync();

                var userDTOs = new List<UserDTO>();

                if (users == null || users?.Count == 0)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Users Not Found!";

                    return response;
                }

                foreach (var user in users)
                {
                    var userDTO = new UserDTO
                    {
                        Id = user.Id,
                        UserRole = (await _userRoleService.GetUserRoleById(user.UserRoleId)).UserRole,
                        Username = user.Username,
                    };

                    userDTOs.Add(userDTO);
                }

                response.ResponseMessage.StatusCode = HttpStatusCode.OK;
                response.Users = userDTOs;

                return response;
            }
            catch (Exception ex)
            {
                response.ResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;

                return response;
            }
        }

        public async Task<UserResponse> GetUserById(int id)
        {
            var response = new UserResponse();
            try
            {
                var User = await _userRepository.GetUserByIdAsync(id);

                if (User == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "User Not Found!";

                    return response;
                }

                var UserDTO = new UserDTO
                {
                    Id = User.Id,
                    UserRole = (await _userRoleService.GetUserRoleById(User.UserRoleId)).UserRole,
                    Username = User.Username,
                };

                response.ResponseMessage.StatusCode = HttpStatusCode.OK;
                response.User = UserDTO;

                return response;
            }
            catch (Exception ex)
            {
                response.ResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;

                return response;
            }
        }

        public async Task<UserResponse> GetUserByUsername(string username)
        {
            var response = new UserResponse();
            try
            {
                var User = await _userRepository.GetUserByUsernameAsync(username);

                if (User == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "User Not Found!";

                    return response;
                }

                var UserDTO = new UserDTO
                {
                    Id = User.Id,
                    UserRole = (await _userRoleService.GetUserRoleById(User.UserRoleId)).UserRole,
                    Username = User.Username,
                };

                response.ResponseMessage.StatusCode = HttpStatusCode.OK;
                response.User = UserDTO;

                return response;
            }
            catch (Exception ex)
            {
                response.ResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;

                return response;
            }
        }

        public async Task<Response> AddUser(UserDTO UserDTO)
        {
            var response = new Response();
            try
            {
                if (string.IsNullOrWhiteSpace(UserDTO.Username))
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Username is Required!";

                    return response;
                }

                if (string.IsNullOrWhiteSpace(UserDTO.Password))
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Password is Required!";

                    return response;
                }

                if (UserDTO.UserRole == null || UserDTO.UserRole?.Id == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "User Role is Required!";

                    return response;
                }

                var userRoleInDb = (await _userRoleService.GetUserRoleById(UserDTO.UserRole.Id)).UserRole;
                if (userRoleInDb == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "User Role Not Found!";

                    return response;
                }

                var userInDb = (await _userRepository.GetUserByUsernameAsync(UserDTO.Username));
                if (userInDb != null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.Conflict;
                    response.Message = $"Username '{UserDTO.Username}' is already taken!";

                    return response;
                }

                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(UserDTO.Password, out passwordHash, out passwordSalt);

                await _userRepository.AddUserAsync(new User 
                {
                    UserRoleId = UserDTO.UserRole.Id,
                    Username = UserDTO.Username,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                });

                response.ResponseMessage.StatusCode = HttpStatusCode.OK;
                response.Message = "Successful Addition!";

                return response;
            }
            catch (Exception ex)
            {
                response.ResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;

                return response;
            }
        }

        public async Task<Response> UpdateUser(UserDTO UserDTO)
        {
            var response = new Response();
            var userRequest = new User();
            userRequest.Id = UserDTO.Id;

            try
            {
                if (UserDTO == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "User is null!";

                    return response;
                }

                var User = await _userRepository.GetUserByIdAsync(UserDTO.Id);
                if (User == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "User Not Found!";

                    return response;
                }

                if (UserDTO.UserRole != null)
                {
                    if (UserDTO.UserRole.Id == null)
                    {
                        response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                        response.Message = "User Role Not Found!";

                        return response;
                    }

                    var userRoleInDb = (await _userRoleService.GetUserRoleById(UserDTO.UserRole.Id)).UserRole;
                    if (userRoleInDb == null)
                    {
                        response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                        response.Message = "User Role Not Found!";

                        return response;
                    }
                    userRequest.UserRoleId = UserDTO.UserRole.Id;
                }

                if (!string.IsNullOrWhiteSpace(UserDTO.Username) && UserDTO.Username != User.Username)
                {
                    var userInDb = (await _userRepository.GetUserByUsernameAsync(UserDTO.Username));
                    if (userInDb != null)
                    {
                        response.ResponseMessage.StatusCode = HttpStatusCode.Conflict;
                        response.Message = $"Username '{UserDTO.Username}' is already taken!";

                        return response;
                    }
                    userRequest.Username = UserDTO.Username;
                }

                if (!string.IsNullOrWhiteSpace(UserDTO.Password))
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash(UserDTO.Password, out passwordHash, out passwordSalt);
                    userRequest.PasswordHash = passwordHash;
                    userRequest.PasswordSalt = passwordSalt;
                }

                await _userRepository.UpdateUserAsync(userRequest);

                response.ResponseMessage.StatusCode = HttpStatusCode.OK;
                response.Message = "Successful Update!";

                return response;
            }
            catch (Exception ex)
            {
                response.ResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;

                return response;
            }
        }

        public async Task<Response> DeleteUser(int id)
        {
            var response = new Response();
            try
            {
                var User = await _userRepository.GetUserByIdAsync(id);

                if (User == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "User Not Found!";

                    return response;
                }

                await _userRepository.DeleteUserAsync(User);

                response.ResponseMessage.StatusCode = HttpStatusCode.OK;
                response.Message = "Successful Deletion!";

                return response;
            }
            catch (Exception ex)
            {
                response.ResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;

                return response;
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
