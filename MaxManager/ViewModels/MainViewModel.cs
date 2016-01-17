using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Windows.Networking;
using Windows.Networking.NetworkOperators;
using Windows.UI.Core;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using MaxManager.Web.Lan;
using MaxManager.Web.Lan.Discovery;
using MaxManager.Web.Lan.Parser;

namespace MaxManager.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		private readonly MaxCubeDiscoverer _maxCubeDiscoverer;

		public MainViewModel()
		{
			_maxCubeDiscoverer = new MaxCubeDiscoverer();
			_maxCubeDiscoverer.CubeDiscovered += _maxCubeDiscoverer_CubeDiscovered;

			ConnectCommand = new RelayCommand(Connect);
			DiscoverCommand = new RelayCommand(Discover);
		}

		public ICommand ConnectCommand { get; set; }
		public ICommand DiscoverCommand { get; set; }

		public string ConnectResult
		{
			get { return _connectResult; }
			set { Set(ref _connectResult, value); }
		}
		private string _connectResult;

		public bool ProgressEnabled
		{
			get { return _progressEnabled; }
			set { Set(ref _progressEnabled, value); }
		}
		private bool _progressEnabled;

		public string ProgressMessage
		{
			get { return _progressMessage; }
			set { Set(ref _progressMessage, value); }
		}
		private string _progressMessage;

		private async void Connect()
		{
			var maxParser = new MaxParser();
			var maxConnector = new MaxConnector("192.168.0.7", maxParser);
			await maxConnector.LoadState();
			ConnectResult = maxConnector.ToString();
		}

		private async void Discover()
		{
			ProgressEnabled = true;
			ProgressMessage = "Discovering Max! Cube on network...";

			_cubes.Clear();

			await _maxCubeDiscoverer.DiscoverCubes();
		}


		private ConcurrentDictionary<HostName, CubeInfo> _cubes = new ConcurrentDictionary<HostName, CubeInfo>();

		private async void _maxCubeDiscoverer_CubeDiscovered(object sender, CubeDiscoveredEventArgs cubeDiscoveredEventArgs)
		{
			_cubes.TryAdd(cubeDiscoveredEventArgs.RemoteAddress, cubeDiscoveredEventArgs.CubeInfo);
			var hostNames = string.Join(",", _cubes.Keys.Select(name => name.DisplayName));

			DispatcherHelper.CheckBeginInvokeOnUI(() =>
			{
				ProgressMessage = string.Format("Discovered {0} Max! Cube(s): {1}", _cubes.Count, hostNames);
			});
		}
	}
}