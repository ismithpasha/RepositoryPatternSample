using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternSample.ClientModels.Models.Auth.Authenticate
{
    public class AuthResponseModel
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }

        public object? User { get; set; }
    }
}
