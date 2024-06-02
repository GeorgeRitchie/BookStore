using System.Reflection;

namespace Infrastructure
{
	public static class AssemblyReference
	{
		public static Assembly Assembly { get; } = typeof(AssemblyReference).Assembly;
	}
}
