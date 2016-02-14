namespace MaxManager.Web.Lan.Parser.Message
{
	public class LMessageHeatingThermostat : LMessage
	{
		public bool IsBatteryLow { get; set; }
		public bool IsDaylightSaving { get; set; }
		public bool IsTransmitError { get; set; }
		public MaxRoomControlMode RoomControlMode { get; set; }
		public double SetPointTemperature { get; set; }

		public override string ToString()
		{
			return
				$"{DeviceType}: Temperature: {SetPointTemperature} Mode: {RoomControlMode}, Is Transmit Error: {IsTransmitError}, Is Battery Low: {IsBatteryLow}, Is Daylight Saving: {IsDaylightSaving}";
		}
	}
}