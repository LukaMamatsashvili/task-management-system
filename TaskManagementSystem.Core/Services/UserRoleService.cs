using Microsoft.EntityFrameworkCore;
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

        public async Task<UserRolesResponse> GetUserRoles()
        {
            var response = new UserRolesResponse();
            try
            {
                var userRoles = await _userRoleRepository.GetUserRolesAsync();

                var userRoleDTOs = new List<UserRoleDTO>();

                if (userRoles == null || userRoles?.Count == 0)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "User Roles Not Found!";

                    return response;
                }

                userRoleDTOs = userRoles.Select(user => new UserRoleDTO
                {
                    Id = user.Id,
                    Type = user.Type,
                }).ToList();

                response.ResponseMessage.StatusCode = HttpStatusCode.OK;
                response.UserRoles = userRoleDTOs;

                return response;
            }
            catch (Exception ex)
            {
                response.ResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;

                return response;
            }
        }

        public async Task<UserRoleResponse> GetUserRoleById(int id)
        {
            var response = new UserRoleResponse();
            try
            {
                var userRole = await _userRoleRepository.GetUserRoleByIdAsync(id);

                if (userRole == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "User Role Not Found!";

                    return response;
                }

                var userRoleDTO = new UserRoleDTO
                {
                    Id = userRole.Id,
                    Type = userRole.Type,
                };

                response.ResponseMessage.StatusCode = HttpStatusCode.OK;
                response.UserRole = userRoleDTO;

                return response;
            }
            catch (Exception ex)
            {
                response.ResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;

                return response;
            }
        }

        public async Task<Response> AddUserRole(UserRoleDTO UserRoleDTO)
        {
            var response = new Response();
            try
            {
                if (UserRoleDTO == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "User Role is Required!";

                    return response;
                }

                if (UserRoleDTO.Type == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "User Role type is Required!";

                    return response;
                }

                var userRoleInDb = await _userRoleRepository.GetUserRoleByTypeAsync(UserRoleDTO.Type);
                if (userRoleInDb != null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.Conflict;
                    response.Message = "This User Role Type already exists!";

                    return response;
                }

                var UserRole = new UserRole
                {
                    Type = UserRoleDTO.Type,
                };

                await _userRoleRepository.AddUserRoleAsync(UserRole);

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

        public async Task<Response> UpdateUserRole(UserRoleDTO UserRoleDTO)
        {
            var response = new Response();
            try
            {
                if (UserRoleDTO == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "User Role is Required!";

                    return response;
                }

                var UserRole = await _userRoleRepository.GetUserRoleByIdAsync(UserRoleDTO.Id);
                if (UserRole == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "User Role Not Found!";

                    return response;
                }

                if (UserRoleDTO.Type == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "User Role type is Required!";

                    return response;
                }

                var userRoleInDb = await _userRoleRepository.GetUserRoleByTypeAsync(UserRoleDTO.Type);
                if (userRoleInDb != null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.Conflict;
                    response.Message = "This User Role Type already exists!";

                    return response;
                }

                await _userRoleRepository.UpdateUserRoleAsync(new UserRole { Id = UserRoleDTO.Id, Type = UserRoleDTO.Type });

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

        public async Task<Response> DeleteUserRole(int id)
        {
            var response = new Response();
            try
            {
                var UserRole = await _userRoleRepository.GetUserRoleByIdAsync(id);

                if (UserRole == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "User Role Not Found!";

                    return response;
                }

                await _userRoleRepository.DeleteUserRoleAsync(UserRole);

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
    }
}
