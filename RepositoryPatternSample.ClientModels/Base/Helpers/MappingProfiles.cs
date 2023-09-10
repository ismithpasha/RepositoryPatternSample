using AutoMapper;
using RepositoryPatternSample.ClientModels.Models.Admin;
using RepositoryPatternSample.ClientModels.Models.Auth.Menu;
using RepositoryPatternSample.ClientModels.Models.Auth.Role;
using RepositoryPatternSample.ClientModels.Models.Auth.User;
using RepositoryPatternSample.Entities.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RepositoryPatternSample.ClientModels.Base
{
    public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			CreateMap<Menu, MenuCreateVm>().ReverseMap();
			CreateMap<Menu, MenuVm>().ReverseMap();
            CreateMap<RoleMenuPermission, RoleMenuPermissionCreateVm>().ReverseMap();
            CreateMap<RoleMenuPermission, RoleMenuPermissionUpdateVm>().ReverseMap();
            //CreateMap<RoleMenuPermission, ROLE>().ReverseMap();
            CreateMap<UserType, UserVm>().ReverseMap();
			CreateMap<UserType, UserTypeVm>().ReverseMap();

            CreateMap<Role, RoleVm>().ReverseMap();
        }
	}
}
