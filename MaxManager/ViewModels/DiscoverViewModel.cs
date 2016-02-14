using System.Collections.Concurrent;
using System.Linq;
using Windows.Networking;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using MaxManager.Model;
using MaxManager.Utils.Timer;
using MaxManager.ViewModels.Common;
using MaxManager.Web.Lan.Discovery;

namespace MaxManager.ViewModels
{
	public class DiscoverViewModel : ViewModelBase, INavigable
	{
		private readonly IMaxCubeDiscoverer _maxCubeDiscoverer;
		private readonly INavigationService _navigationService;
		private readonly ConcurrentDictionary<HostName, CubeInfo> _cubes;

		public DiscoverViewModel(IMaxCubeDiscoverer maxCubeDiscoverer, INavigationService navigationService)
		{
			_maxCubeDiscoverer = maxCubeDiscoverer;
			_navigationService = navigationService;

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
		countdownTimer.Start(1);
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