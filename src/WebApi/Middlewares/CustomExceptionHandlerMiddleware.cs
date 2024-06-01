using Domain.Shared.ExceptionAbstractions;
using WebApi.Models.ResponseModels;

namespace WebApi.Middlewares
{
	public class CustomExceptionHandlerMiddleware
	{
		private readonly RequestDelegate next;
		private readonly IDictionary<Type, Action<HttpContext, Exception>> exceptionHandlers;

		public CustomExceptionHandlerMiddleware(RequestDelegate next)
		{
			this.next = next;

			// Register known exception types and handlers here.
			exceptionHandlers = new Dictionary<Type, Action<HttpContext, Exception>>
			{
			};
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		private Task HandleExceptionAsync(HttpContext context, Exception ex)
		{
			Type type = ex.GetType();

			if (exceptionHandlers.ContainsKey(type))
				exceptionHandlers[type].Invoke(context, ex);
			else if (ex is IPublicException pubEx)
				DefaultPublicExceptionHandler(context, pubEx);
			else if (ex is IInternalException inEx)
				DefaultInternalExceptionHandler(context, inEx);
			else
				HandleUnknownException(context, ex);

			return Task.CompletedTask;
		}

		#region Default exception handlers

		private void HandleUnknownException(HttpContext context, Exception ex)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = StatusCodes.Status500InternalServerError;
			context.Response.WriteAsJsonAsync(new ExceptionDataAsResponse
			{
				StatusCode = "500",
				Message = "Internal server error. Connect to support team.",
				Details = ""
			});

			Log.Error("Unknown exception caught. Details: {@ex}", ex);
		}

		private void DefaultInternalExceptionHandler(HttpContext context, IInternalException ex)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = StatusCodes.Status500InternalServerError;
			context.Response.WriteAsJsonAsync(new ExceptionDataAsResponse
			{
				StatusCode = ex.StatusCode.Value,
				Message = ex.Message,
				Details = ""
			});

			Log.Warning("Internal exception with code '{@code}' caught. Exception thrown in '{@source}' with message '{@message}'\nDetails: {@details}\nException object: {@ex}",
							ex.StatusCode,
							ex.Source,
							ex.Message,
							ex.Description,
							ex);
		}

		private void DefaultPublicExceptionHandler(HttpContext context, IPublicException ex)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = StatusCodes.Status400BadRequest;
			context.Response.WriteAsJsonAsync(new ExceptionDataAsResponse
			{
				StatusCode = ex.StatusCode.Value,
				Message = ex.Message,
				Details = ex.Description
			});

			Log.Warning("Public exception with code '{@code}' caught. Exception thrown in '{@source}' with message '{@message}'\nDetails: {@details}\nException object: {@ex}",
							ex.StatusCode,
							ex.Source,
							ex.Message,
							ex.Description,
							ex);
		}

		#endregion

		#region Registered exceptions handlers

		// add your custom exception handlers here, don't forget to add logging

		#endregion
	}
}
