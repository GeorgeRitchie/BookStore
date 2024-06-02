using System.Reflection;

namespace Application.Common.Mappings
{
	/// <summary>
	/// Represents an AutoMapper profile that automatically applies mappings from assemblies.
	/// </summary>
	public class AssemblyMappingProfile : Profile
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyMappingProfile"/> class which adds all mapping rules in a specific assembly to created AutoMapper profile.
		/// </summary>
		/// <param name="assembly">The assembly to scan for mappings.</param>
		public AssemblyMappingProfile(Assembly assembly)
		{
			ApplyMappingsFromAssembly(assembly);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyMappingProfile"/> class which adds all mapping rules from all loaded assemblies in the current AppDomain to created AutoMapper profile.
		/// </summary>
		public AssemblyMappingProfile()
		{
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				try
				{
					ApplyMappingsFromAssembly(assembly);
				}
				catch // Skip if any exception
				{ }
			}
		}

		/// <summary>
		/// Applies AutoMapper mappings from a specified assembly by scanning for types that implement <see cref="IMapWith{TProfile}"/>.
		/// </summary>
		/// <param name="assembly">The assembly to scan for mappings.</param>
		private void ApplyMappingsFromAssembly(Assembly assembly)
		{
			var types = assembly.GetExportedTypes()
								.Where(t => t.GetInterfaces()
											 .Any(i => i.IsGenericType
													   && i.GetGenericTypeDefinition() == typeof(IMapWith<>)))
								.ToList();

			foreach (var type in types)
			{
				var instance = Activator.CreateInstance(type);

				var methodInfo = type.GetMethod("Mapping")
					?? type.GetInterface(typeof(IMapWith<>).Name)!.GetMethod("Mapping");

				methodInfo?.Invoke(instance, new object[] { this });
			}
		}
	}
}
