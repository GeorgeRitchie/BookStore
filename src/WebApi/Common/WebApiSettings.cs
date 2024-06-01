namespace WebApi.Common
{
	public class WebApiSettings
	{
		public const string ConfigurationSection = "WebApi";

		public string LogFilesDirectory { get; set; } = string.Empty;
	}
}
