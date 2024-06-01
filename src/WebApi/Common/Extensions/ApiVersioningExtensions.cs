using Asp.Versioning;

namespace WebApi.Common.Extensions
{
	public static class ApiVersioningExtensions
	{
		public static IServiceCollection AddAPIVersioning(this IServiceCollection services)
		{
			services.AddApiVersioning(setup =>
			{
				setup.AssumeDefaultVersionWhenUnspecified = true;
				setup.DefaultApiVersion = new ApiVersion(1, 0);
				setup.ReportApiVersions = true;
			}).AddApiExplorer(setup =>
			{
				setup.GroupNameFormat = "'v'VVV";
				setup.SubstituteApiVersionInUrl = true;
			});

			return services;
		}
	}
}
