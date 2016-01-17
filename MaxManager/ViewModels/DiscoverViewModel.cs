using System.Collections.Concurrent;
using System.Linq;
using System.Windows.Input;
using Windows.Networking;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MaxManager.Utils.Timer;
using MaxManager.ViewModels.Common;
using MaxManager.Web.Lan.Discovery;

namespace MaxManager.ViewModels
{
	public class DiscoverViewModel : ViewModelBase, INavigable
	{
		private readonly MaxCubeDiscoverer _maxCubeDiscoverer;
		private readonly INavigationService _navigationService;
		private readonly ConcurrentDictionary<HostName, CubeInfo> _cubes;

		public DiscoverViewModel(MaxCubeDiscoverer maxCubeDiscoverer, INavigationService navigationService)
		{
			_maxCubeDiscoverer = maxCubeDiscoverer;
			_navigationService = navigationService;

			_cubes = new ConcurrentDictionary<HostName, CubeInfo>();

			_maxCubeDiscoverer.CubeDiscovered += (sender, cubeDiscoveredEventArgs) => {
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
					_navigationService.NavigateTo(NavigationKeys.Home, keyValuePair.Key);
				}
			};
			countdownTimer.Start(5);
		}

		public void Activate(object parameter)
		{
			Discover();
		}

		public void Deactivate(object parameter)
		{
		}
	}
}