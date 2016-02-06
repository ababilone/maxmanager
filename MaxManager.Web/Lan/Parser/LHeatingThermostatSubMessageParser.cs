using System;
using MaxManager.Web.Lan.Parser.Message;
using MaxManager.Web.Lan.Serialization;
using MaxManager.Web.State;

namespace MaxManager.Web.Lan.Parser
{
	public class LHeatingThermostatSubMessageParser : ILSubMessageParser
	{
		private readonly MaxSerializer _maxSerializer;

		public LHeatingThermostatSubMessageParser(MaxSerializer maxSerializer)
		{
			_maxSerializer = maxSerializer;
		}

		public bool Accept(byte[] data)
		{
			return data.Length == 6;
		}

		public LMessage Parse(byte[] data)
		{
			var heatingThermostatDeviceState = _maxSerializer.Deserialize<HeatingThermostatDeviceState>(data);
			var lMessageHeatingThermostat = new LMessageHeatingThermostat
			{
				DeviceType = MaxDeviceType.HeatingThermostat,
				IsBatteryLow = heatingThermostatDeviceState.IsBatteryLow,
				IsDaylightSaving = heatingThermostatDeviceState.IsDaylightSaving,
				IsTransmitError = heatingThermostatDeviceState.IsTransmitError,
				RoomControlMode = (MaxRoomControlMode)heatingThermostatDeviceState.RoomControlMode,
				SetPointTemperature = heatingThermostatDeviceState.SetPointTemperature / 2.0
			};

			return lMessageHeatingThermostat;
		}

		private DateTime ExtractDate(byte a, byte b)
		{
			var year = (b & 0x1f);
			var day = (a & 0x1f);
			var month = (a & 0xE0) >> 4 | (b & 0x40) >> 6;

			if (year > 0 && month > 0 && day > 0)
				return new DateTime(year, 0, day);
			return DateTime.MaxValue;
		}
	}
}