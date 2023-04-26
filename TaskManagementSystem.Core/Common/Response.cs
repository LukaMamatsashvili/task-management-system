using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.DTOs;
using TaskManagementSystem.Infrastructure.Models;

namespace TaskManagementSystem.Core.Common
{
    public class Response
    {
        public Response()
        {
            ResponseMessage = new HttpResponseMessage();
            Message = "";
        }

        public HttpResponseMessage ResponseMessage { get; set; }
        public string Message { get; set; }
    }


    public class UserRolesResponse : Response
    {
        public List<UserRoleDTO> UserRoles { get; set; }
    }
    public class UserRoleResponse : Response
    {
        public UserRoleDTO UserRole { get; set; }
    }


    public class UsersResponse : Response
    {
        public List<UserDTO> Users { get; set; }
    }
    public class UserResponse : Response
    {
        public UserDTO User { get; set; }
    }


    public class TasksResponse : Response
    {
        public List<TaskDTO> Tasks { get; set; }
    }
    public class TaskResponse : Response
    {
        public TaskDTO Task { get; set; }
    }


    public class TaskAttachmentsResponse : Response
    {
        public List<TaskAttachmentDTO> TaskAttachments { get; set; }
    }
    public class TaskAttachmentResponse : Response
    {
        public TaskAttachmentDTO TaskAttachment { get; set; }
    }
}
