namespace WebApi.Models.ResponseModels
{
	public class FailureResponse
	{
		public string Message { get; set; } = string.Empty;
		public string Details { get; set; } = string.Empty;
		public string StatusCode { get; set; } = string.Empty;
	}
}
