using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IUserPermissionLinkRepository _userPermissionLinkRepository;
        private readonly IPermissionService _permissionService;

        public UserService(IUserRepository userRepository, IUserRoleService userRoleService, 
            IUserPermissionLinkRepository userPermissionLinkRepository, IPermissionService permissionService)
        {
            _userRepository = userRepository;
            _userRoleService = userRoleService;
            _userPermissionLinkRepository = userPermissionLinkRepository;
            _permissionService = permissionService;
        }

        public async Task<List<UserDTO>> GetUsers()
        {
            var users = await _userRepository.GetUsersAsync();

            var userDTOs = new List<UserDTO>();

            if (users == null || users?.Count == 0)
                return userDTOs;

            foreach (var user in users)
            {
                var userDTO = new UserDTO
                {
                    Id = user.Id,
                    UserRole = await _userRoleService.GetUserRoleById(user.Id),
                    Username = user.Username,
                    //Password = 
                };
                userDTO.Permissions = await GetPermissionsByUserId(user.Id);

                userDTOs.Add(userDTO);
            }

            return userDTOs.ToList();
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            var User = await _userRepository.GetUserByIdAsync(id);

            if (User == null)
                return new UserDTO();

            var UserDTO = new UserDTO
            {
                Id = User.Id,
                UserRole = await _userRoleService.GetUserRoleById(User.Id),
                Username = User.Username,
            };
            UserDTO.Permissions = await GetPermissionsByUserId(User.Id);

            return UserDTO;
        }

        public async Task<UserDTO> GetUserByUsername(string username)
        {
            var User = await _userRepository.GetUserByUsernameAsync(username);

            if (User == null)
                return new UserDTO();

            var UserDTO = new UserDTO
            {
                Id = User.Id,
                UserRole = await _userRoleService.GetUserRoleById(User.Id),
                Username = User.Username,
            };
            UserDTO.Permissions = await GetPermissionsByUserId(User.Id);

            return UserDTO;
        }

        private async Task<List<PermissionDTO>> GetPermissionsByUserId(int userId)
        {
            var userPermissionLinks = await _userPermissionLinkRepository.GetUserPermissionLinksByUserIdAsync(userId);

            var PermissionDTOs = new List<PermissionDTO>();

            if (userPermissionLinks == null || userPermissionLinks?.Count == 0)
                return PermissionDTOs;

            foreach (var userPermissionLink in userPermissionLinks)
            {
                var PermissionDTO = await _permissionService.GetPermissionById(userPermissionLink.PermissionId);

                if (PermissionDTO == null)
                    continue;

                PermissionDTOs.Add(PermissionDTO);
            }

            return PermissionDTOs;
        }

        public async Task<string> AddUser(UserDTO UserDTO)
        {
            if (string.IsNullOrWhiteSpace(UserDTO.Username))
                throw new AppException("Username is required!");

            if (string.IsNullOrWhiteSpace(UserDTO.Password))
                throw new AppException("Password is required!");

            if (UserDTO.UserRole == null)
                throw new AppException("UserRole is required!");

            if (UserDTO.UserRole.Id == null)
                throw new AppException("UserRole is required!");

            if ((await _userRepository.GetUserByUsernameAsync(UserDTO.Username))?.Username != null)
                throw new AppException("Username \"" + UserDTO.Username + "\" is already taken!");

            var User = new User     
            {
                UserRoleId = UserDTO.UserRole.Id,
                Username = UserDTO.Username,
            };

            if (UserDTO.Permissions != null && UserDTO.Permissions?.Count != 0)
            {
                foreach (var Permission in UserDTO.Permissions)
                {
                    await _userPermissionLinkRepository.AddUserPermissionLinkAsync(new UserPermissionLink
                    {
                        UserId = UserDTO.Id,
                        PermissionId = Permission.Id,
                    });
                }
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(UserDTO.Password, out passwordHash, out passwordSalt);

            User.PasswordHash = passwordHash;
            User.PasswordSalt = passwordSalt;

            await _userRepository.AddUserAsync(User);

            return "Successful addition!";
        }

        public async Task<string> UpdateUser(UserDTO UserDTO)
        {
            if (UserDTO == null)
                throw new AppException("User is null!");

            var User = await _userRepository.GetUserByUsernameAsync(UserDTO.Username);

            if (User == null)
                throw new AppException("User not found");

            if (UserDTO.UserRole != null)
            {
                if (!string.IsNullOrWhiteSpace(UserDTO.UserRole.Type))
                    User.UserRoleId = UserDTO.UserRole.Id;
            }

            if (!string.IsNullOrWhiteSpace(UserDTO.Username) && UserDTO.Username != User.Username)
            {
                if ((await _userRepository.GetUserByUsernameAsync(UserDTO.Username)).Username != null)
                    throw new AppException("Username " + UserDTO.Username + " is already taken");

                User.Username = UserDTO.Username;
            }

            if (!string.IsNullOrWhiteSpace(UserDTO.Password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(UserDTO.Password, out passwordHash, out passwordSalt);

                User.PasswordHash = passwordHash;
                User.PasswordSalt = passwordSalt;
            }

            if (UserDTO.Permissions != null)
            {
                await _userPermissionLinkRepository.DeleteUserPermissionLinksByUserId(User.Id);

                foreach (var Permission in UserDTO.Permissions)
                {
                    await _userPermissionLinkRepository.AddUserPermissionLinkAsync(new UserPermissionLink
                    {
                        UserId = User.Id,
                        PermissionId = Permission.Id,
                    });
                }
            }

            await _userRepository.UpdateUserAsync(User);

            return "Successful update!";
        }

        public async Task<string> DeleteUser(int id)
        {
            await _userRepository.DeleteUserAsync(id);

            return "Successful deletion!";
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
