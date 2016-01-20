using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using MaxManager.Web.Lan;
using MaxManager.Web.Lan.Parser;
using MaxManager.Web.State;

namespace MaxManager.ViewModels
{
	public class MainViewModel : ViewModelBase, INavigable
	{
		private readonly INavigationService _navigationService;

		public MainViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;

			ConnectCommand = new RelayCommand(Connect);
			Rooms = new ObservableCollection<MaxRoom>();
		}

		public ICommand ConnectCommand { get; set; }

		public ObservableCollection<MaxRoom> Rooms { get; set; }

		public string ConnectResult
		{
			get { return _connectResult; }
			set { Set(ref _connectResult, value); }
		}
		private string _connectResult;

		private async void Connect()
		{
			var maxParser = new MaxParser();
			var maxConnector = new MaxConnector("192.168.0.7", maxParser);
			maxConnector.StateUpdated += MaxConnectorOnStateUpdated;
			await maxConnector.LoadState();
			ConnectResult = maxConnector.ToString();
		}

		private void MaxConnectorOnStateUpdated(object sender, StateUpdatedEventArgs stateUpdatedEventArgs)
		{
			DispatcherHelper.CheckBeginInvokeOnUI(() =>
			{
				Rooms.Clear();
				foreach (var maxRoom in stateUpdatedEventArgs.Rooms)
				{
					Rooms.Add(maxRoom);
				}
			});
		}

		public void Activate(object parameter)
		{
			Connect();
		}

		public void Deactivate(object parameter)
		{
		}
	}
}