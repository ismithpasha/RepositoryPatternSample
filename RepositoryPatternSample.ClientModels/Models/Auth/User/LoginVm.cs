using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternSample.ClientModels.Models.Auth.User
{
    public class LoginVm
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
