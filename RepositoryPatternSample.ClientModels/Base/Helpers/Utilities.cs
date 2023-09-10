using RepositoryPatternSample.ClientModels.Base.Resources;
using RepositoryPatternSample.ClientModels.Models.Auth.Authenticate;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RepositoryPatternSample.ClientModels.Base.AppConstants;

namespace RepositoryPatternSample.ClientModels.Base.Helpers
{
    public static class Utilities
	{
		public static ResponseModel GetSuccessMsg(string message, object data = null)
		{
			return new ResponseModel
			{
				IsSuccess = true,
				StatusCode = StatusCodes.Status200OK,
				Status = ResultStatus.Success.ToString(),
				Message = message,
				Data = data
			};
		}

		public static ResponseModel GetAlreadyExistMsg(string? module = "Data")
		{
			return new ResponseModel
			{
				IsSuccess = false,
				StatusCode = StatusCodes.Status409Conflict,
				Status = ResultStatus.Canceled.ToString(),
				Message = module + " already exist!",
				Data = null
			};
		}

		public static ResponseModel GetErrorMsg(string message)
		{
			return new ResponseModel
			{
				IsSuccess = false,
				StatusCode = StatusCodes.Status400BadRequest,
				Status = ResultStatus.Success.ToString(),
				Message = message
			};
		}

		public static ResponseModel GetInternalServerErrorMsg(Exception ex, string customMessage = null)
		{
			return new ResponseModel
			{
				IsSuccess = false,
				StatusCode = StatusCodes.Status500InternalServerError,
				Status = ResultStatus.Error.ToString(),
				Message = string.IsNullOrWhiteSpace(customMessage) ? (ex.Message + (ex.InnerException != null ? " --> InnerException: " + ex.InnerException.Message : "")) : customMessage,
				Data = null
			};
		}

		public static ResponseModel GetInternalServerErrorMsg(string message)
		{
			return new ResponseModel
			{
				IsSuccess = false,
				StatusCode = StatusCodes.Status500InternalServerError,
				Status = ResultStatus.Error.ToString(),
				Message = message,
				Data = null
			};
		}

		public static ResponseModel GetNoDataFoundMsg(string? msg = "No data found!")
		{
			return new ResponseModel
			{
				IsSuccess = false,
				StatusCode = StatusCodes.Status404NotFound,
				Status = ResultStatus.Error.ToString(),
				Message = msg,
				Data = null
			};
		}

		public static ResponseModel GetUnauthenticatedMsg()
		{
			return new ResponseModel
			{
				IsSuccess = false,
				StatusCode = StatusCodes.Status401Unauthorized,
				Status = ResultStatus.Error.ToString(),
				Message = CommonMessages.Unauthenticated,
				Data = null
			};
		}

		public static ResponseModel GetUnauthorizedMsg()
		{
			return new ResponseModel
			{
				IsSuccess = false,
				StatusCode = StatusCodes.Status403Forbidden,
				Status = ResultStatus.Error.ToString(),
				Message = CommonMessages.Unauthorized,
				Data = null
			};
		}

		public static ResponseModel GetValidationFailedMsg(string msg)
		{
			return new ResponseModel
			{
				IsSuccess = false,
				StatusCode = StatusCodes.Status400BadRequest,
				Status = ResultStatus.Error.ToString(),
				Message = CommonMessages.ValidationFailed,
				Data = msg
			};
		}

		public static ResponseModel GetValidationFailedMsg(List<string> msg)
		{
			return new ResponseModel
			{
				IsSuccess = false,
				StatusCode = StatusCodes.Status400BadRequest,
				Status = ResultStatus.Error.ToString(),
				Message = CommonMessages.ValidationFailed,
				Data = msg
			};
		}
	}
}
