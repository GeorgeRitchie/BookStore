using Microsoft.Extensions.DependencyInjection;

namespace Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			// Adding AutoMapper
			services.AddAutoMapper(config => config.AddProfile(new AssemblyMappingProfile()));

			// Adding FluentValidation and validators from all loaded assemblies in the current AppDomain
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				try
				{
					services.AddValidatorsFromAssembly(assembly);
				}
				catch { } // Skip if any exception
			}

			return services;
		}
	}
}
