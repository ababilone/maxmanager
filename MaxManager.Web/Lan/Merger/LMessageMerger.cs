using System.Linq;
using MaxManager.Web.Lan.Parser.Message;
using MaxManager.Web.State;

namespace MaxManager.Web.Lan.Merger
{
	public class LMessageMerger : IMessageMerger
	{
		public bool Accept(object message)
		{
			return message is LMessages;
		}

		public void Merge(MaxCube maxCube, object message)
		{
			var lMessages = message as LMessages;
			if (lMessages == null)
				return;

			foreach (var lMessage in lMessages.Messages)
			{
				var maxDevice = maxCube.Rooms.SelectMany(room => room.Devices).FirstOrDefault(device => device.RfAddress == lMessage.RfAddress);
				if (maxDevice != null)
				{
					if (lMessage.DeviceType == MaxDeviceType.HeatingThermostat)
					{
						var lMessageHeatingThermostat = lMessage as LMessageHeatingThermostat;
						if (lMessageHeatingThermostat == null)
							continue;

						maxDevice.SetPointTemperature = lMessageHeatingThermostat.SetPointTemperature;
						maxDevice.RoomControlMode = lMessageHeatingThermostat.RoomControlMode;
						maxDevice.State = new DeviceState
						{
							BatteryLow = lMessageHeatingThermostat.IsBatteryLow,
							TransmitError = lMessageHeatingThermostat.IsTransmitError
						};
					}
				}
			}
		}
	}
}