namespace Application.Common.Attributes
{
	/// <summary>
	/// An attribute that allows anonymous access to a class or its members, typically used in the context of authentication and authorization.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class AllowAnonymousAttribute : Attribute
	{
	}
}
