namespace Application.Common.Attributes
{
	// See https://github.com/jasontaylordev/CleanArchitecture

	/// <summary>
	/// Specifies the class this attribute is applied to requires authorization.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class AuthorizeAttribute : Attribute
	{
		/// <summary>
		/// Gets or sets a comma delimited list of roles that are allowed to access the resource.
		/// </summary>
		public string Roles { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the policy name that determines access to the resource.
		/// </summary>
		public string Policies { get; set; } = string.Empty;

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthorizeAttribute"/> class. 
		/// </summary>
		public AuthorizeAttribute() { }
	}
}
