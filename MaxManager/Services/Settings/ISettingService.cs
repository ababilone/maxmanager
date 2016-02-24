namespace MaxManager.Services.Settings
{
	public interface ISettingService
	{
		bool IsDiscoveryEnabled { get; set; }
		int DiscoveryTimeOut { get; set; }
		string CubeAddress { get; set; }
		bool IsDebugEnabled { get; set; }
		string Theme { get; set; }
		event SettingUpdatedEventHandler SettingUpdated;
	}
}