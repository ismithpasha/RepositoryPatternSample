using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternSample.ClientModels.Models.Auth.Authenticate
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public int? StatusCode { get; set; }
        public string? Status { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
