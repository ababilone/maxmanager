using System;
using GalaSoft.MvvmLight;

namespace MaxManager.ViewModels
{
	public class SettingsViewModel : ViewModelBase
	{
		public int DiscoveryTimeOut
		{
			get { return _discoveryTimeOut; }
			set { Set(ref _discoveryTimeOut, value); }
		}

		private int _discoveryTimeOut;

		public bool IsDebugEnabled
		{
			get { return _isDebugEnabled; }
			set { Set(ref _isDebugEnabled, value); }
		}
		private bool _isDebugEnabled;

		public string Theme
		{
			get { return _theme; }
			set { Set(ref _theme, value); }
		}
		private string _theme;
	}
}