using Domain.Shared;

namespace WebApi.Common.Errors
{
	public static class WebApiErrors
	{
		public static class Common
		{
			public static Error OperationFailed(string message) => new("WebApi.Common.OperationFailed", message);
		}
	}
}
