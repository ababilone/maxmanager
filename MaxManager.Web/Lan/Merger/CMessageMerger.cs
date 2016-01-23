using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using MaxManager.Web.Lan.Parser.Message;
using MaxManager.Web.State;

namespace MaxManager.Web.Lan.Merger
{
	class CMessageMerger : MessageMerger<CMessage>
	{
		protected override void Merge(MaxCube maxCube, CMessage message)
		{
			var cMessageCube = message as CMessageCube;
			var cMessageHeatingThermostat = message as CMessageHeatingThermostat;
			var cMessageWallThermostat = message as CMessageWallThermostat;

			if (cMessageCube != null)
			{
				maxCube.IsPortalEnabled = cMessageCube.IsPortalEnabled;
				maxCube.PortalUrl = cMessageCube.PortalUrl;
				maxCube.TimeZoneDaylightSavings = cMessageCube.TimeZoneDaylightSavings;
				maxCube.TimeZoneWinter = cMessageCube.TimeZoneWinter;
			}
			else if (cMessageHeatingThermostat != null)
			{
				var maxDevice = maxCube.Rooms.SelectMany(room => room.Devices).FirstOrDefault(device => device.RfAddress == cMessageHeatingThermostat.AddressOfDevice);
				if (maxDevice != null)
				{
					maxDevice.BoostDuration = cMessageHeatingThermostat.BoostDuration;
					maxDevice.BoostPercentage = cMessageHeatingThermostat.BoostPercentage;
					maxDevice.ComfortTemperature = cMessageHeatingThermostat.ComfortTemperature;
					maxDevice.DecalcificationDay = cMessageHeatingThermostat.DecalcificationDay;
					maxDevice.DecalcificationTime = cMessageHeatingThermostat.DecalcificationTime;
					maxDevice.EcoTemperature = cMessageHeatingThermostat.EcoTemperature;
					maxDevice.FirmewareVersion = cMessageHeatingThermostat.FirmewareVersion;
					maxDevice.MaxSetPointTemperature = cMessageHeatingThermostat.MaxSetPointTemperature;
					maxDevice.MaxValeSetting = cMessageHeatingThermostat.MaxValeSetting;
					maxDevice.WeekTemperatureProfile = cMessageHeatingThermostat.WeekTemperatureProfile;
					maxDevice.MinSetTemperature = cMessageHeatingThermostat.MinSetTemperature;
					maxDevice.SerialNumber = cMessageHeatingThermostat.SerialNumber;
					maxDevice.TemperatureOffset = cMessageHeatingThermostat.TemperatureOffset;
					maxDevice.ValveOffset = cMessageHeatingThermostat.ValveOffset;
					maxDevice.WindowOpenDuration = cMessageHeatingThermostat.WindowOpenDuration;
					maxDevice.WindowOpenTemperature = cMessageHeatingThermostat.WindowOpenTemperature;
				}
				else
				{
					// Unknown device !?
				}
			}
			else if (cMessageWallThermostat != null)
			{

			}
			else
			{
				throw new NotSupportedException("Merge on " + message.GetType().Name);
			}
		}
	}
}