using Application.Common.Exceptions;
using Domain.Shared.ExceptionAbstractions;
using Newtonsoft.Json;
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
				{ typeof(FluentValidationException), HandleFluentValidationException },
				{ typeof(UnauthenticatedUserException), HandleUnauthenticatedUserException },
				{ typeof(AccessForbiddenException), HandleAccessForbiddenException },
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
			context.Response.WriteAsJsonAsync(new FailureResponse
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
			context.Response.WriteAsJsonAsync(new FailureResponse
			{
				StatusCode = ex.StatusCode.Value,
				Message = ex.Message,
				Details = ""
			});

			Log.Information("Internal exception with code '{@code}' caught. Exception thrown in '{@source}' with message '{@message}'\nDetails: {@details}\nException object: {@ex}",
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
			context.Response.WriteAsJsonAsync(new FailureResponse
			{
				StatusCode = ex.StatusCode.Value,
				Message = ex.Message,
				Details = ex.Description
			});

			Log.Information("Public exception with code '{@code}' caught. Exception thrown in '{@source}' with message '{@message}'\nDetails: {@details}\nException object: {@ex}",
							ex.StatusCode,
							ex.Source,
							ex.Message,
							ex.Description,
							ex);
		}

		#endregion

		#region Registered exceptions handlers

		private void HandleFluentValidationException(HttpContext context, Exception ex)
		{
			FluentValidationException fvex = ex as FluentValidationException;
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = StatusCodes.Status400BadRequest;
			context.Response.WriteAsJsonAsync(new FailureResponse
			{
				StatusCode = fvex!.StatusCode.Value,
				Message = fvex.Message,
				Details = $"{{ \"Errors\": {JsonConvert.SerializeObject(fvex.Errors)} }}"
			});

			Log.Information("Fluent validation exception with code '{@code}' caught. Exception thrown in '{@source}' with message '{@message}'\nDetails: {@details}\nException object: {@ex}",
				   fvex.StatusCode,
				   fvex.Source,
				   fvex.Message,
				   $"{{ \"Errors\": {JsonConvert.SerializeObject(fvex.Errors)} }}",
				   fvex);
		}

		private void HandleUnauthenticatedUserException(HttpContext context, Exception ex)
		{
			UnauthenticatedUserException uuaex = ex as UnauthenticatedUserException;
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = StatusCodes.Status401Unauthorized;
			context.Response.WriteAsJsonAsync(new FailureResponse
			{
				StatusCode = uuaex!.StatusCode.Value,
				Message = uuaex.Message,
				Details = ""
			});

			Log.Information("Unauthenticated user exception with code '{@code}' caught. Exception thrown in '{@source}' with message '{@message}'\nDetails: {@details}\nException object: {@ex}",
				   uuaex.StatusCode,
				   uuaex.Source,
				   uuaex.Message,
				   uuaex.Description,
				   uuaex);
		}

		private void HandleAccessForbiddenException(HttpContext context, Exception ex)
		{
			AccessForbiddenException faex = ex as AccessForbiddenException;
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = StatusCodes.Status403Forbidden;
			context.Response.WriteAsJsonAsync(new FailureResponse
			{
				StatusCode = faex!.StatusCode.Value,
				Message = faex.Message,
				Details = ""
			});

			Log.Information("Access forbidden exception with code '{@code}' caught. Exception thrown in '{@source}' with message '{@message}'\nDetails: {@details}\nException object: {@ex}",
				   faex.StatusCode.Value,
				   faex.Source,
				   faex.Message,
				   faex.Description,
				   faex);
		}

		// add your custom exception handlers here, don't forget to add logging

		#endregion
	}
}
