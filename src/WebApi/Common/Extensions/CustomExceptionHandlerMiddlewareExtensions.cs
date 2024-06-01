using WebApi.Middlewares;

namespace WebApi.Common.Extensions
{
	public static class CustomExceptionHandlerMiddlewareExtensions
	{
		public static IApplicationBuilder UseCustomExceptionHandlerMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
		}
	}
}
