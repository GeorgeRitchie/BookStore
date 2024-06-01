namespace Domain.Shared
{
	public static class ResultExtensions
	{
		public static Task<Result> ToTask(this Result result) => Task.FromResult(result);
		public static Task<Result<TValue?>> ToTask<TValue>(this Result<TValue?> result) => Task.FromResult(result);
	}
}
