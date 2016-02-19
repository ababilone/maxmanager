using System.Windows.Input;
using GalaSoft.MvvmLight;
using MaxManager.Commands;
using MaxManager.Web.Lan;
using MaxManager.Web.Lan.Parser;
using MaxManager.Web.State;

namespace MaxManager.ViewModels
{
	public class RoomViewModel : ViewModelBase
	{
		public RoomViewModel(IMaxConnector maxConnector)
		{
			SetRoomControlModeToAutoCommand = new SetRoomControlModeCommand(maxConnector, MaxRoomControlMode.Auto);
			SetRoomControlModeToManualCommand = new SetRoomControlModeCommand(maxConnector, MaxRoomControlMode.Manual);
		}

		public MaxRoom MaxRoom { get; set; }

		public ICommand SetRoomControlModeToAutoCommand { get; }
		public ICommand SetRoomControlModeToManualCommand { get; }

		public bool IsRoomControlModeAuto
		{
			get { return _isRoomControlModeAuto; }
			set { Set(ref _isRoomControlModeAuto, value); }
		}
		private bool _isRoomControlModeAuto;

		public bool IsRoomControlModeManual
		{
			get { return _isRoomControlModeManual; }
			set { Set(ref _isRoomControlModeManual, value); }
		}
		private bool _isRoomControlModeManual;

		public double SetPointTemperature
		{
			get { return _setPointTemperature; }
			set { Set(ref _setPointTemperature, value); }
		}
		private double _setPointTemperature;

		public string Name
		{
			get { return _name; }
			set { Set(ref _name, value); }
		}
		private string _name;

		public void Update(MaxRoom maxRoom)
		{
			MaxRoom = maxRoom;
			Name = maxRoom.Name;
			SetPointTemperature = maxRoom.SetPointTemperature;
			IsRoomControlModeAuto = maxRoom.RoomControlMode == MaxRoomControlMode.Auto;
			IsRoomControlModeManual = maxRoom.RoomControlMode == MaxRoomControlMode.Manual;
		}
	}
}