using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using MaxManager.Model;
using MaxManager.Web.Lan;
using MaxManager.Web.State;

namespace MaxManager.ViewModels
{
	public class MainViewModel : ViewModelBase, INavigable
	{
		private readonly IMaxConnector _maxConnector;
		private readonly INavigationService _navigationService;

		public MainViewModel(IMaxConnector maxConnector, INavigationService navigationService)
		{
			_maxConnector = maxConnector;
			_navigationService = navigationService;

			_maxConnector.StateUpdated += MaxConnectorOnStateUpdated;

			//ConnectCommand = new RelayCommand(() => Connect());
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

		private async void Connect(CubeInfoWithHostName cubeInfoWithHostName)
		{
			await _maxConnector.Connect(cubeInfoWithHostName.HostName.ToString());
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
			var cubeInfoWithHostName = parameter as CubeInfoWithHostName;
			if (cubeInfoWithHostName != null)
			{
				Connect(cubeInfoWithHostName);
			}
		}

		public void Deactivate(object parameter)
		{
		}
	}
}