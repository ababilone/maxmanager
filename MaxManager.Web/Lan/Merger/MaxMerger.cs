using System.Collections.Generic;
using System.Linq;
using MaxManager.Web.Lan.Parser.Message;
using MaxManager.Web.State;

namespace MaxManager.Web.Lan.Merger
{
	public class MaxMerger
	{
		private readonly List<IMessageMerger> _messageMergers;

		public MaxMerger()
		{
			_messageMergers = new List<IMessageMerger>
			{
				new HMessageMerger(),
				new MMessageMerger(),
				new CMessageMerger(),
				new LMessageMerger()
			};
		}

		public void Merge(MaxCube maxCube, object message)
		{
			var messageParser = _messageMergers.FirstOrDefault(merger => merger.Accept(message));
			messageParser?.Merge(maxCube, message);
		}
	}

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