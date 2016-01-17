using System.Collections.Concurrent;
using System.Linq;
using System.Windows.Input;
using Windows.Networking;
using GalaSoft.MvvmLight.Command;
using MaxManager.Utils.Timer;
using MaxManager.ViewModels.Common;
using MaxManager.Web.Lan.Discovery;

namespace MaxManager.ViewModels
{
	public class DiscoverViewModel
	{
		private readonly MaxCubeDiscoverer _maxCubeDiscoverer;
		private readonly ConcurrentDictionary<HostName, CubeInfo> _cubes;

		public DiscoverViewModel(MaxCubeDiscoverer maxCubeDiscoverer)
		{
			_maxCubeDiscoverer = maxCubeDiscoverer;
			_cubes = new ConcurrentDictionary<HostName, CubeInfo>();

			_maxCubeDiscoverer.CubeDiscovered += (sender, cubeDiscoveredEventArgs) => {
				_cubes.TryAdd(cubeDiscoveredEventArgs.RemoteAddress, cubeDiscoveredEventArgs.CubeInfo);
			};

			Progress = new Progress();
			DiscoverCommand = new RelayCommand(Discover);
		}

		public Progress Progress { get; }
		public ICommand DiscoverCommand { get; set; }

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
			};
			countdownTimer.Start(5);
		}
	}
}