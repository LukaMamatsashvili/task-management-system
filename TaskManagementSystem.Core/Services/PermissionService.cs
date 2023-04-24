using Microsoft.EntityFrameworkCore;
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
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<List<PermissionDTO>> GetPermissions()
        {
            var Permissions = await _permissionRepository.GetPermissionsAsync();

            var PermissionDTOs = new List<PermissionDTO>();

            if (Permissions == null || Permissions?.Count == 0)
                return PermissionDTOs;

            PermissionDTOs = Permissions.Select(user => new PermissionDTO
            {
                Id = user.Id,
                Type = user.Type,
            }).ToList();

            return PermissionDTOs;
        }

        public async Task<PermissionDTO> GetPermissionById(int id)
        {
            var Permission = await _permissionRepository.GetPermissionByIdAsync(id);

            if (Permission == null)
                return new PermissionDTO();

            var PermissionDTO = new PermissionDTO
            {
                Id = Permission.Id,
                Type = Permission.Type,
            };

            return PermissionDTO;
        }

        public async Task<string> AddPermission(PermissionDTO PermissionDTO)
        {
            if (PermissionDTO == null)
                throw new Exception("Permission is null!");

            if (PermissionDTO.Type == null)
                throw new Exception("Permission type is null!");

            var Permission = new Permission
            {
                Type = PermissionDTO.Type,
            };

            await _permissionRepository.AddPermissionAsync(Permission);

            return "Successful addition!";
        }

        public async Task<string> UpdatePermission(PermissionDTO PermissionDTO)
        {
            if (PermissionDTO == null)
                throw new AppException("Permission is null!");

            var Permission = await _permissionRepository.GetPermissionByIdAsync(PermissionDTO.Id);

            if (Permission == null)
                throw new AppException("Permission not found!");

            if(PermissionDTO.Type != null)
                Permission.Type = PermissionDTO.Type;

            await _permissionRepository.UpdatePermissionAsync(Permission);

            return "Successful update!";
        }

        public async Task<string> DeletePermission(int id)
        {
            await _permissionRepository.DeletePermissionAsync(id);

            return "Successful deletion!";
        }
    }
}
