using GalaSoft.MvvmLight;
using MaxManager.Services.Settings;

namespace MaxManager.ViewModels
{
	public class SettingsViewModel : ViewModelBase
	{
		private readonly ISettingService _settingService;

		public SettingsViewModel(ISettingService settingService)
		{
			_settingService = settingService;
			_settingService.SettingUpdated += (service, args) => Load();
			Load();
		}

		private void Load()
		{
			RaisePropertyChanged(() => IsDiscoveryEnabled);
			RaisePropertyChanged(() => DiscoveryTimeOut);
			RaisePropertyChanged(() => CubeAddress);
			RaisePropertyChanged(() => IsDebugEnabled);
			RaisePropertyChanged(() => Theme);
		}

		public bool IsDiscoveryEnabled
		{
			get { return _settingService.IsDiscoveryEnabled; }
			set { _settingService.IsDiscoveryEnabled = value; RaisePropertyChanged(); }
		}

		public int DiscoveryTimeOut
		{
			get { return _settingService.DiscoveryTimeOut; }
			set { _settingService.DiscoveryTimeOut = value; RaisePropertyChanged(); }
		}

		public string CubeAddress
		{
			get { return _settingService.CubeAddress; }
			set { _settingService.CubeAddress = value; RaisePropertyChanged(); }
		}

		public bool IsDebugEnabled
		{
			get { return _settingService.IsDebugEnabled; }
			set { _settingService.IsDebugEnabled = value; RaisePropertyChanged(); }
		}

		public string Theme
		{
			get { return _settingService.Theme; }
			set { _settingService.Theme = value; RaisePropertyChanged(); }
		}
	}
}