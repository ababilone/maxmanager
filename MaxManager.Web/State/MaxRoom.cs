using System;
using System.Collections.Generic;

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

		public int Order { get; set; }

		public int BoostDuration { get; set; }

		public int BoostValveAngle { get; set; }

		public float ComfortTemperature { get; set; }

		public float EcoTemperature { get; set; }

		public float MaximumTemperature { get; set; }

		public float SetPointTemperature { get; set; }

		public bool SetPointTemperatureValid { get; set; }

		public string ControlMode { get; set; }

		public string DecalcificationDay { get; set; }

		public int DecalcificationHour { get; set; }

		public List<MaxDevice> Devices { get; set; }

		public bool StateChanged { get; set; }

		public bool StateDirty { get; set; }

		public bool TemperatureControllable { get; set; }

		public string TemperatureMode { get; set; }

		public DateTime TemporaryModeStopDate { get; set; }

		public MaxWeekTemperatureProfile WeekTemperatureProfile { get; set; }

		public int WindowOpenDuration { get; set; }

		public float WindowOpenTemperature { get; set; }

		public int MaximumNoOfHeatingThermostats { get; set; }

		public int MaximumNoOfShutterContacts { get; set; }

		public int MaximumNoOfWallMountedThermostats { get; set; }

		public float CurrentAutoTemperature { get; set; }

		public float ActualTemperature { get; set; }

		public string GroupRfAddress { get; set; }

		public MaxCubeState Cube { get; set; }

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
	}
}