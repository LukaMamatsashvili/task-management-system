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
        private readonly IPermissionRepository _PermissionRepository;

        public PermissionService(IPermissionRepository PermissionRepository)
        {
            _PermissionRepository = PermissionRepository;
        }

        public async Task<List<PermissionDTO>> GetPermissions()
        {
            var Permissions = await _PermissionRepository.GetPermissionsAsync();

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
            var Permission = await _PermissionRepository.GetPermissionByIdAsync(id);

            if (Permission == null)
                return new PermissionDTO();

            var PermissionDTO = new PermissionDTO
            {
                Id = Permission.Id,
                Type = Permission.Type,
            };

            return PermissionDTO;
        }

        public async Task<int> AddPermission(PermissionDTO PermissionDTO)
        {
            if (PermissionDTO == null)
                throw new Exception("Permission is null!");

            if (PermissionDTO.Type == null)
                throw new Exception("Permission type is null!");

            var Permission = new Permission
            {
                Id = PermissionDTO.Id,
                Type = PermissionDTO.Type,
            };

            var PermissionId = await _PermissionRepository.AddPermissionAsync(Permission);

            return PermissionId;
        }

        public async Task<string> UpdatePermission(PermissionDTO PermissionDTO)
        {
            if (PermissionDTO == null)
                throw new Exception("Permission is null!");

            if (PermissionDTO.Type == null)
                throw new Exception("Permission type is null!");

            var Permission = new Permission
            {
                Id = PermissionDTO.Id,
                Type = PermissionDTO.Type,
            };

            await _PermissionRepository.UpdatePermissionAsync(Permission);

            return "Successful update!";
        }

        public async Task<string> DeletePermission(int id)
        {
            await _PermissionRepository.DeletePermissionAsync(id);

            return "Successful update!";
        }
    }
}
