using GalaSoft.MvvmLight;

namespace MaxManager.ViewModels.Common
{
	public class Progress : ObservableObject
	{
		public bool IsEnabled
		{
			get { return _isEnabled; }
			set { Set(ref _isEnabled, value); }
		}
		private bool _isEnabled;

		public string PrimaryMessage
		{
			get { return _primaryMessage; }
			set { Set(ref _primaryMessage, value); }
		}
		private string _primaryMessage;

		public string SecondaryMessage
		{
			get { return _secondaryMessage; }
			set { Set(ref _secondaryMessage, value); }
		}
		private string _secondaryMessage;
	}
}