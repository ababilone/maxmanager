using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using MaxManager.Model;
using MaxManager.Web.Lan;
using MaxManager.Web.Lan.Events;
using MaxManager.Web.Lan.Parser.Message;
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

			//ConnectCommand = new RelayCommand(() => Connect());
			Rooms = new ObservableCollection<MaxRoom>();
			MaxEvents = new ObservableCollection<MaxEvent>();

			_maxConnector.StateUpdated += MaxConnectorOnStateUpdated;
			_maxConnector.MessageReceived += (sender, args) => AddMaxEvent(new MaxEvent("< " + args.MaxMessage.ToString()));
			_maxConnector.CommandSent += (sender, args) => AddMaxEvent(new MaxEvent("> "+  args.MaxCommand.ToString()));
			_maxConnector.Connected += (sender, args) => AddMaxEvent(new MaxEvent("@ Connected to " + args.Host));
			_maxConnector.ExceptionThrowed += (sender, args) => AddMaxEvent(new MaxEvent("@ Exception throwed: " + args.Exception.Message));
		}

		public ICommand ConnectCommand { get; set; }

		public ObservableCollection<MaxRoom> Rooms { get; set; }
		public ObservableCollection<MaxEvent> MaxEvents { get; } 

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

		private void AddMaxEvent(MaxEvent maxEvent)
		{
			Task.Run(() => DispatcherHelper.UIDispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { MaxEvents.Insert(0, maxEvent); }));
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