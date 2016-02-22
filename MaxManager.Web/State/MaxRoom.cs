using System.Collections.Generic;
using System.Linq;
using MaxManager.Web.Lan.Parser;

namespace MaxManager.Web.State
{
	public class MaxRoom
	{
		public MaxRoom()
		{
			Devices = new List<MaxDevice>();
		}

		public int Id { get; set; }

		public string Name { get; set; }

		public List<MaxDevice> Devices { get; set; }

		public MaxWeekTemperatureProfile WeekTemperatureProfile { get; set; }

		public MaxCube Cube { get; set; }

		public double SetPointTemperature => Devices.FirstOrDefault()?.SetPointTemperature ?? 0;
		public MaxRoomControlMode RoomControlMode => Devices.FirstOrDefault()?.RoomControlMode ?? MaxRoomControlMode.Auto;
		public bool IsBatteryLow => Devices.Any(device => device.State?.BatteryLow ?? false);
		public bool IsTransmitError => Devices.Any(device => device.State?.TransmitError ?? false);

		public MaxRfAddress GroupRfAddress { get; set; }
	}
}