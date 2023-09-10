using RepositoryPatternSample.ClientModels.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternSample.ClientModels.Models.Auth.User
{
    public class UserVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string? Address { get; set; }
        public int? TypeId { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Password { get; set; }
        public int? UpdatedBy { get; set; }
        public int RoleId { get; set; }
    }

    public class UserListVm : CommonVmProperties
    {
        public int Id { get; set; }
        public int? TypeId { get; set; }
        public string? TypeName { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; } 
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; } 
        public int TotalElements { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
