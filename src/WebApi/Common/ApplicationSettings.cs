namespace WebApi.Common
{
	public class ApplicationSettings
	{
		public const string ConfigurationSection = "WebApi";

		public string LogFilesDirectory { get; set; } = string.Empty;
	}
}
