using System.Windows.Input;
using Windows.Networking.NetworkOperators;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaxManager.Web.Lan;
using MaxManager.Web.Lan.Parser;

namespace MaxManager.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		public MainViewModel()
		{
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

		private async void Connect()
		{
			var maxParser = new MaxParser();
			var maxConnector = new MaxConnector("192.168.0.7", maxParser);
			await maxConnector.LoadState();
			ConnectResult = maxConnector.ToString();
		}

		private async void Discover()
		{
			var maxConnector = new MaxConnector("", new MaxParser());
			await maxConnector.DiscoverCubes();
		}
	}
}