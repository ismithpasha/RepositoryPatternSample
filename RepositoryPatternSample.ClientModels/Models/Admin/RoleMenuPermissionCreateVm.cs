using RepositoryPatternSample.Entities.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternSample.ClientModels.Models.Admin
{
    public class RoleMenuPermissionCreateVm
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int MenuId { get; set; }
    }
}