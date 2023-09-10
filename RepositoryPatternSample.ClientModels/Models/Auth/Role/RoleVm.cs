using RepositoryPatternSample.ClientModels.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternSample.ClientModels.Models.Auth.Role
{
    public class RoleVm
    {
        public int Id { get; set; } 
        public string Name { get; set; } 
        public string Description { get; set; }
    } 

    public class RoleCreateVm
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }

    public class RoleListVm : CommonVmProperties
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
