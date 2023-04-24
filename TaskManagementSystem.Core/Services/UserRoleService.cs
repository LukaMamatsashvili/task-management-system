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
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleService(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        public async Task<List<UserRoleDTO>> GetUserRoles()
        {
            var userRoles = await _userRoleRepository.GetUserRolesAsync();

            var userRoleDTOs = new List<UserRoleDTO>();

            if (userRoles == null || userRoles?.Count == 0)
                return userRoleDTOs;

            userRoleDTOs = userRoles.Select(user => new UserRoleDTO
            {
                Id = user.Id,
                Type = user.Type,
            }).ToList();

            return userRoleDTOs;
        }

        public async Task<UserRoleDTO> GetUserRoleById(int id)
        {
            var userRole = await _userRoleRepository.GetUserRoleByIdAsync(id);

            if (userRole == null)
                return new UserRoleDTO();

            var userRoleDTO = new UserRoleDTO
            {
                Id = userRole.Id,
                Type = userRole.Type,
            };

            return userRoleDTO;
        }

        public async Task<string> AddUserRole(UserRoleDTO UserRoleDTO)
        {
            if (UserRoleDTO == null)
                throw new Exception("UserRole is null!");

            if(UserRoleDTO.Type == null)
                throw new Exception("UserRole type is null!");

            var UserRole = new UserRole
            {
                Type = UserRoleDTO.Type,
            };

            await _userRoleRepository.AddUserRoleAsync(UserRole);

            return "Successful addition!";
        }

        public async Task<string> UpdateUserRole(UserRoleDTO UserRoleDTO)
        {
            if (UserRoleDTO == null)
                throw new AppException("UserRole is null!");

            var UserRole = await _userRoleRepository.GetUserRoleByIdAsync(UserRoleDTO.Id);

            if (UserRole == null)
                throw new AppException("UserRole not found!");

            if (UserRoleDTO.Type != null)
                UserRole.Type = UserRoleDTO.Type;

            await _userRoleRepository.UpdateUserRoleAsync(UserRole);

            return "Successful update!";
        }

        public async Task<string> DeleteUserRole(int id)
        {
            await _userRoleRepository.DeleteUserRoleAsync(id);

            return "Successful deletion!";
        }
    }
}
