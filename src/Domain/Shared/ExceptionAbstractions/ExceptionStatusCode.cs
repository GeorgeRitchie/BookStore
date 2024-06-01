namespace Domain.Shared.ExceptionAbstractions
{
	public abstract class ExceptionStatusCode : Enumeration<ExceptionStatusCode, string>
	{
		protected ExceptionStatusCode(string name, string value) : base(name, value)
		{
		}

		protected sealed override bool IsValueEqual(string? first, string? second)
		{
			return first == second;
		}
	}
}
