using Serilog.Events;
using Serilog;
using Microsoft.Extensions.Options;

namespace WebApi.Common.Extensions
{
	public static class SerilogExtensions
	{
		public static IServiceCollection AddSerilogStuff(this IServiceCollection services)
		{
			var appSettings = services.BuildServiceProvider().GetRequiredService<IOptions<ApplicationSettings>>().Value;

			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Is(LogEventLevel.Warning)
				.WriteTo.File(Path.Combine(appSettings.LogFilesDirectory, "ProgramLog-.txt"),
								rollingInterval: RollingInterval.Day)
				.CreateLogger();

			services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger));

			return services;
		}
	}
}
