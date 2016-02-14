using System;
using MaxManager.Web.State;

namespace MaxManager.Web.Lan.Parser.Message
{
	public class CMessageHeatingThermostat : CMessage
	{
		public MaxWeekTemperatureProfile WeekTemperatureProfile { get; set; }
		public double ValveOffset { get; set; }
		public double MaxValeSetting { get; set; }
		public TimeSpan DecalcificationTime { get; set; }
		public DayOfWeek DecalcificationDay { get; set; }
		public double BoostPercentage { get; set; }
		public TimeSpan BoostDuration { get; set; }
		public TimeSpan WindowOpenDuration { get; set; }
		public double WindowOpenTemperature { get; set; }
		public double TemperatureOffset { get; set; }
		public double MinSetTemperature { get; set; }
		public double MaxSetPointTemperature { get; set; }
		public double EcoTemperature { get; set; }
		public double ComfortTemperature { get; set; }
		public string SerialNumber { get; set; }
		public int TestResult { get; set; }
		public int FirmewareVersion { get; set; }
		public int RoomId { get; set; }
		public string AddressOfDevice { get; set; }
		public int DataLength { get; set; }

		public override string ToString()
		{
			return $"{DeviceType} ({RfAddress}), Min °: {MinSetTemperature} Max °: {MaxSetPointTemperature} Eco °: {EcoTemperature} Comfort°: {ComfortTemperature}, Boost duration: {BoostDuration}, Decalcification on {DecalcificationDay} @ {DecalcificationTime}";
		}
	}
}