using System.Windows.Input;
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
		}

		public ICommand ConnectCommand { get; set; }
		
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
	}
}