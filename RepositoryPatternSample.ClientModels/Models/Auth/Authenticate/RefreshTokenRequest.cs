using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternSample.ClientModels.Models.Auth.Authenticate
{
    public class RefreshTokenRequest
    {
        [Required]
        public string ExpiredToken { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
