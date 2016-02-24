using System;
using Windows.Storage;

namespace MaxManager.Services.Settings
{
	public class SettingService : ISettingService
	{
		private readonly ApplicationDataContainer _roamingSettings;

		public SettingService()
		{
			_roamingSettings = ApplicationData.Current.RoamingSettings;

			ApplicationData.Current.DataChanged += CurrentOnDataChanged;
		}

		private void CurrentOnDataChanged(ApplicationData sender, object args)
		{
			FireSettingsUpdated();
		}

		private void FireSettingsUpdated()
		{
			SettingUpdated?.Invoke(this, new EventArgs());
		}

		public event SettingUpdatedEventHandler SettingUpdated;

		private T Get<T>(string key, T defaultValue)
		{
			if (_roamingSettings.Values.ContainsKey(key))
				return (T)_roamingSettings.Values[key];
			return defaultValue;
		}

		public bool IsDiscoveryEnabled
		{
			get { return Get(nameof(IsDiscoveryEnabled), true); }
			set { _roamingSettings.Values[nameof(IsDiscoveryEnabled)] = value; FireSettingsUpdated(); }
		}

		public int DiscoveryTimeOut
		{
			get { return Get(nameof(DiscoveryTimeOut), 1); }
			set { _roamingSettings.Values[nameof(DiscoveryTimeOut)] = value; FireSettingsUpdated(); }
		}

		public string CubeAddress
		{
			get { return Get(nameof(CubeAddress), ""); }
			set { _roamingSettings.Values[nameof(CubeAddress)] = value; FireSettingsUpdated(); }
		}

		public bool IsDebugEnabled
		{
			get { return Get(nameof(IsDebugEnabled), false); }
			set { _roamingSettings.Values[nameof(IsDebugEnabled)] = value; FireSettingsUpdated(); }
		}

		public string Theme
		{
			get { return Get(nameof(Theme), "light"); }
			set { _roamingSettings.Values[nameof(Theme)] = value; FireSettingsUpdated(); }
		}
	}
}