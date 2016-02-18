using System.Windows.Input;
using GalaSoft.MvvmLight;
using MaxManager.Commands;
using MaxManager.Web.Lan;
using MaxManager.Web.State;

namespace MaxManager.ViewModels
{
	public class RoomViewModel : ViewModelBase
	{
		public RoomViewModel(IMaxConnector maxConnector)
		{
			SetRoomControlModeCommand = new SetRoomControlModeCommand(maxConnector);
		}

		public MaxRoom MaxRoom { get; set; }

		public ICommand SetRoomControlModeCommand { get; }

		private double _setPointTemperature;
		public double SetPointTemperature
		{
			get { return _setPointTemperature; }
			set { Set(ref _setPointTemperature, value); }
		}

		private string _name;
		public string Name
		{
			get { return _name; }
			set { Set(ref _name, value); }
		}

		public void Update(MaxRoom maxRoom)
		{
			MaxRoom = maxRoom;
			Name = maxRoom.Name;
			SetPointTemperature = maxRoom.SetPointTemperature;
		}
	}
}