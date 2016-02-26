using System.Collections.Concurrent;
using System.Linq;
using Windows.Networking;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using MaxManager.Model;
using MaxManager.Services.Settings;
using MaxManager.Utils.Timer;
using MaxManager.ViewModels.Common;
using MaxManager.Web.Lan.Discovery;

namespace MaxManager.ViewModels
{
	public class DiscoverViewModel : ViewModelBase, INavigable
	{
		private readonly IMaxCubeDiscoverer _maxCubeDiscoverer;
		private readonly INavigationService _navigationService;
		private readonly ISettingService _settingService;
		private readonly ConcurrentDictionary<HostName, CubeInfo> _cubes;

		public DiscoverViewModel(IMaxCubeDiscoverer maxCubeDiscoverer, INavigationService navigationService, ISettingService settingService)
		{
			_maxCubeDiscoverer = maxCubeDiscoverer;
			_navigationService = navigationService;
			_settingService = settingService;

			_cubes = new ConcurrentDictionary<HostName, CubeInfo>();

			_maxCubeDiscoverer.CubeDiscovered += (sender, cubeDiscoveredEventArgs) =>
			{
				_cubes.TryAdd(cubeDiscoveredEventArgs.RemoteAddress, cubeDiscoveredEventArgs.CubeInfo);
			};

			Progress = new Progress();
		}

		public Progress Progress { get; }

		private async void Discover()
		{
			Progress.IsEnabled = true;
			Progress.PrimaryMessage = "Discovering Max! Cube on network...";

			_cubes.Clear();

			await _maxCubeDiscoverer.DiscoverCubes();

			var countdownTimer = new CountdownTimer();
			countdownTimer.CountdownSecondElasped += (sender, seconds) =>
			{
				Progress.SecondaryMessage = string.Format("{0}s remaining", seconds);
			};
			countdownTimer.CountdownFinished += sender =>
			{
				var hostNames = string.Join(",", _cubes.Keys.Select(name => name.DisplayName));
				Progress.PrimaryMessage = string.Format("Discovered {0} Max! Cube{1} @ {2}", _cubes.Count, _cubes.Count > 1 ? "s" : "", hostNames);
				Progress.SecondaryMessage = "";
				Progress.IsEnabled = false;

				if (_cubes.Count == 1)
				{
					var keyValuePair = _cubes.FirstOrDefault();
					var cubeInfoWithHostName = new CubeInfoWithHostName
					{
						CubeInfo = keyValuePair.Value,
						HostName = keyValuePair.Key
					};

					_navigationService.NavigateTo(NavigationKeys.Home, cubeInfoWithHostName);
				}
			};
			countdownTimer.Start(_settingService.DiscoveryTimeOut);
		}

		public void Activate(object parameter)
		{
			if (_settingService.IsDiscoveryEnabled)
			{
				Discover();
			}
			else if (!string.IsNullOrEmpty(_settingService.CubeAddress))
			{
				var cubeInfoWithHostName = new CubeInfoWithHostName
				{
					HostName = new HostName(_settingService.CubeAddress)
				};
				_navigationService.NavigateTo(NavigationKeys.Home, cubeInfoWithHostName);
			}
		}

		public void Deactivate(object parameter)
		{
		}
	}
}