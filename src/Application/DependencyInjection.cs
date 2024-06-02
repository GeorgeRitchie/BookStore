using Application.Common.Behaviors;
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

			// Adding Mediator
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

			// Registration mediator pipelines
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));

			return services;
		}
	}
}
