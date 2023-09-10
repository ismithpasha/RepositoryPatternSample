using RepositoryPatternSample.Api.Configurations;

namespace RepositoryPatternSample.Api.ServiceExtensions
{
	public static class ApplicationBuilderExtension
	{
		public static IApplicationBuilder AddGlobalErrorHandler(this IApplicationBuilder applicationBuilder)
		=> applicationBuilder.UseMiddleware<GlobalErrorHandlingMiddleware>();
	}
}
