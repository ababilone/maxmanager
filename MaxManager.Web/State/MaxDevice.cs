using System;

namespace MaxManager.Web.State
{
	public class MaxDevice
	{
		public MaxDeviceType Type { get; set; }

		public string RfAddress { get; set; }

		public string SerialNumber { get; set; }

		public string Name { get; set; }

		public string RadioState { get; set; }

		public DeviceState State { get; set; }

		public string StateInfo { get; set; }

		public MaxRoom Room { get; set; }

		public bool IsRadioOk => IsRadioStateOk && !State.TransmitError;

		public bool IsRadioStateOk => RadioState =="Ok";
		public TimeSpan BoostDuration { get; set; }
		public double BoostPercentage { get; set; }
		public double ComfortTemperature { get; set; }
		public DayOfWeek DecalcificationDay { get; set; }
		public TimeSpan DecalcificationTime { get; set; }
		public double EcoTemperature { get; set; }
		public int FirmewareVersion { get; set; }
		public double MaxSetPointTemperature { get; set; }
		public double MaxValeSetting { get; set; }
		public MaxWeekTemperatureProfile WeekTemperatureProfile { get; set; }
		public double MinSetTemperature { get; set; }
		public double TemperatureOffset { get; set; }
		public double ValveOffset { get; set; }
		public TimeSpan WindowOpenDuration { get; set; }
		public double WindowOpenTemperature { get; set; }
	}
}
