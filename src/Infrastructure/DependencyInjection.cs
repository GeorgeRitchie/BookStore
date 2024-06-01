using Application.Common.Interfaces.Repositories;
using Infrastructure.Common;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services)
		{
			// Adding DataBase
			services.AddDbContext<AppDbContext>((serviceProvider, options) =>
			{
				string path = Directory.GetCurrentDirectory();
				var appSettings = serviceProvider.GetService<IOptions<InfrastructureSettings>>()!.Value;
				options.UseSqlServer(appSettings.DbConnectionString.Replace("[DataDirectory]", path));
			});

			// Adding Repository and Unit of work with Repository patterns implementations
			services.AddScoped<IAppDb, AppDb>();

			return services;
		}
	}
}
