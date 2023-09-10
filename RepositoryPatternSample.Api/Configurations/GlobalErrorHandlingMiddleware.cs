using RepositoryPatternSample.ClientModels.Models.Auth.Authenticate;
using RepositoryPatternSample.ClientModels.Models.Utilities;
using System.Net;
using System.Text.Json;
using static RepositoryPatternSample.ClientModels.Base.AppConstants;

namespace RepositoryPatternSample.Api.Configurations
{
	public class GlobalErrorHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

		public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, exception.Message);
				await HandleExceptionAsync(context, exception);
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception exception)
		{

			context.Response.ContentType = "application/json";
			var response = context.Response;

            var errorResponse = new ResponseModel
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Status = ResultStatus.Error.ToString(),
                Message = exception.InnerException != null ? exception.InnerException.Message : exception.Message,
                Data = exception.Data.ToString()
            };



            switch (exception.GetType().Name)
			{
				case nameof(BadRequestException):
					response.StatusCode = (int)HttpStatusCode.BadRequest;
					break;
				case nameof(NotFoundException):
					response.StatusCode = (int)HttpStatusCode.NotFound;
					break;
				case nameof(NotImplementedException):
					response.StatusCode = (int)HttpStatusCode.NotImplemented;
					break;
				case nameof(UnauthorizedAccessException):
					response.StatusCode = (int)HttpStatusCode.Unauthorized;
					break;
				case nameof(KeyNotFoundException):
					response.StatusCode = (int)HttpStatusCode.NotFound;
					break;
				default:
					response.StatusCode = (int)HttpStatusCode.InternalServerError;
					break;
			}




			return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
		}
	}
}
