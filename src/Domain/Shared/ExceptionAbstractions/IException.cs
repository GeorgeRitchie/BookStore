namespace Domain.Shared.ExceptionAbstractions
{
	public interface IException
	{
		public ExceptionStatusCode StatusCode { get; }
		public string Message { get; }
		public string Description { get; }
		public string Source { get; }
	}
}
