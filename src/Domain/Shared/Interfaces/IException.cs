namespace Domain.Shared.Interfaces
{
	public interface IException
	{
		public string StatusCode { get; }
		public string Message { get; }
		public string Description { get; }
		public string Source { get; }
	}
}
