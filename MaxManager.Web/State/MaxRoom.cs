using System;
using System.Collections.Generic;
using System.Linq;

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

		public TimeSpan BoostDuration { get; set; }

		public double ComfortTemperature { get; set; }

		public double EcoTemperature { get; set; }

		public float MaximumTemperature { get; set; }

		public string ControlMode { get; set; }

		public DayOfWeek DecalcificationDay { get; set; }

		public List<MaxDevice> Devices { get; set; }

		public string TemperatureMode { get; set; }

		public MaxWeekTemperatureProfile WeekTemperatureProfile { get; set; }

		public TimeSpan WindowOpenDuration { get; set; }

		public double WindowOpenTemperature { get; set; }

		public float ActualTemperature { get; set; }

		public string GroupRfAddress { get; set; }

		public MaxCube Cube { get; set; }

		public List<float> GetSettableTemperatures()
		{
			var ret = new List<float>();
			var max = MaximumTemperature;
			for (var i = 5; i <= max; i++)
			{
				ret.Add(Convert.ToSingle(i));
				var iPlus = i + 0.5f;
				if (iPlus <= max && iPlus > 17)
				{
					ret.Add(Convert.ToSingle(iPlus));
				}
			}
			return ret;
		}

		public bool IsAutoOrEco => IsControlModeAuto || IsTemperatureModeEco;

		public bool IsActualTemperatureAvailable => ActualTemperature > 0;

		public bool IsTemperatureModeNormal => TemperatureMode == "Normal";

		public bool IsTemperatureModeEco => TemperatureMode == "Eco";

		public bool IsTemperatureModeComfort => TemperatureMode == "Comfort";

		public bool IsControlModeAuto => ControlMode == "Auto";

		public bool IsControlModePermanently => ControlMode == "Permanently";

		public bool IsControlModeTemporary => ControlMode == "Temporary";

		public double TemperatureOffset => Devices.FirstOrDefault().TemperatureOffset;

		public double ValveOffset { get; set; }

		public string SerialNumber { get; set; }

		public double MinSetTemperature { get; set; }

		public double MaxValeSetting { get; set; }

		public double MaxSetPointTemperature { get; set; }

		public int FirmewareVersion { get; set; }

		public TimeSpan DecalcificationTime { get; set; }

		public double BoostPercentage { get; set; }
	}
}