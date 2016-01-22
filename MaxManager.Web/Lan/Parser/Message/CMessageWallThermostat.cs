using MaxManager.Web.State;

namespace MaxManager.Web.Lan.Parser.Message
{
	public class CMessageWallThermostat
	{
		public MaxWeekTemperatureProfile MaxWeekTemperatureProfile { get; set; }
		public double MinSetTemperature { get; set; }
		public double MaxSetPointTemperature { get; set; }
		public double EcoTemperature { get; set; }
		public double ComfortTemperature { get; set; }
		public string SerialNumber { get; set; }
		public int TestResult { get; set; }
		public int FirmewareVersion { get; set; }
		public int RoomId { get; set; }
		public MaxDeviceType DeviceType { get; set; }
		public string AddressOfDevice { get; set; }
		public int DataLength { get; set; }
	}
}