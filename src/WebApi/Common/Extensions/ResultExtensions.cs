using Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApi.Models.ResponseModels;

namespace WebApi.Common.Extensions
{
	public static class ResultExtensions
	{
		public static IActionResult To200Or400Or500Response(this Result? result)
		{
			if (result == null)
			{
				return new ObjectResult(new FailureResponse { StatusCode = "500", Message = "Internal server error. Connect to support team.", Details = "" })
				{
					StatusCode = StatusCodes.Status500InternalServerError
				};
			}
			else if (result.IsSuccess == false)
			{
				return new BadRequestObjectResult(new FailureResponse()
				{
					StatusCode = "FailureResponse",
					Message = "Operation failed. See Details.",
					Details = JsonConvert.SerializeObject(result.Errors)
				});
			}
			else
			{
				return new OkResult();
			}
		}

		public static IActionResult To200Or400Or500Response<T>(this Result<T>? result)
		{
			if (result == null)
			{
				return new ObjectResult(new FailureResponse { StatusCode = "500", Message = "Internal server error. Connect to support team.", Details = "" })
				{
					StatusCode = StatusCodes.Status500InternalServerError
				};
			}
			else if (result.IsSuccess == false)
			{
				return new BadRequestObjectResult(new FailureResponse()
				{
					StatusCode = "FailureResponse",
					Message = "Operation failed. See Details.",
					Details = JsonConvert.SerializeObject(result.Errors)
				});
			}
			else
			{
				return new OkObjectResult(result.Value);
			}
		}
	}
}
