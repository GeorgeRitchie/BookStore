namespace Infrastructure.Common
{
	public class InfrastructureSettings
	{
		public const string ConfigurationSection = "Infrastructure";

		public string Environment { get; set; } = string.Empty;
		public bool IsDevelopmentEnvironment { get; set; } = false;
		public string DbConnectionString { get; set; } = string.Empty;
	}
}
