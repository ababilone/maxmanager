using System;
using System.Linq;
using MaxManager.Web.Lan.Parser.Message;
using MaxManager.Web.State;

namespace MaxManager.Web.Lan.Parser
{
	public class CMessageParser : IMessageParser
	{
		public bool Accept(string payload)
		{
			return payload.StartsWith("C:");
		}

		public IMaxMessage Parse(string payload)
		{
			var address = payload.Substring(2).Split(',')[0];
			var rfAddress = FormatRfAddress(address);
			var dataAsString = payload.Substring(2).Split(',')[1];
			var data = Convert.FromBase64String(dataAsString);

			var deviceType = (MaxDeviceType)data[4];
			switch (deviceType)
			{
				case MaxDeviceType.Cube:
					return ParseCube(data, rfAddress);
				case MaxDeviceType.HeatingThermostat:
				case MaxDeviceType.HeatingThermostatPlus:
					return ParseHeatingThermostat(data, rfAddress);
				case MaxDeviceType.WallThermostat:
					return ParseWallThermostat(data, rfAddress);
			}
			return null;
		}

		private static string FormatRfAddress(string address)
		{
			return (address[0] + address[1] + "-" + address[2] + address[3] + "-" + address[4] + address[5]).ToUpper();
		}

		public CMessageCube ParseCube(byte[] data, string rfAddress)
		{
			var isPortalEnabled = data[0] > 0;
			var deviceType = (MaxDeviceType)data[4];
			var portalUrl = System.Text.Encoding.UTF8.GetString(data, 85, 128);
			var timeZoneWinter = data.Skip(213).Take(5).ToArray();
			var timeZoneDaylightSavings = data.Skip(226).Take(5).ToArray();

			return new CMessageCube
			{
				RfAddress = rfAddress,
				DeviceType = deviceType,
				IsPortalEnabled = isPortalEnabled,
				PortalUrl = portalUrl,
				TimeZoneWinter = timeZoneWinter,
				TimeZoneDaylightSavings = timeZoneDaylightSavings
			};
		}

		public CMessageHeatingThermostat ParseHeatingThermostat(byte[] data, string rfAddress)
		{
			var dataLength = (int)data[0];
			var addressOfDevice = BitConverter.ToString(data, 1, 3);
			var deviceType = (MaxDeviceType)data[4];
			var roomId = (int)data[5];
			var firmewareVersion = (int)data[6];
			var testResult = (int)data[7];
			var serialNumber = BitConverter.ToString(data, 8, 10);
			var comfortTemperature = data[18] * 2.0;
			var ecoTemperature = data[19] * 2.0;
			var maxSetPointTemperature = data[20] * 2.0;
			var minSetTemperature = data[21] * 2.0;
			var temperatureOffset = data[22] * 2.0 + 3.5;
			var windowOpenTemperature = data[23] * 2.0;
			var windowOpenDuration = TimeSpan.FromMinutes(data[24] / 5.0);

			var boost = data[25];

			var boostDuration = TimeSpan.FromMinutes((boost >> 5) * 5);
			var boostPercentage = (boost & 0x1f) * 5.0 / 100;

			var decalcification = data[26];
			var decalcificationDay = GetDayOfWeek(decalcification >> 5);
			var decalcificationTime = TimeSpan.FromHours(decalcification & 0x1f);

			var maxValeSetting = data[27] * 100.0 / 255.0;
			var valveOffset = data[28] * 100.0 / 255.0;

			var weeklyProgram = data.Skip(28).ToArray();

			var maxWeekTemperatureProfile = ParseWeekProgram(weeklyProgram);

			return new CMessageHeatingThermostat
			{
				RfAddress = rfAddress,
				DeviceType = deviceType,
				DataLength = dataLength,
				AddressOfDevice = addressOfDevice,
				RoomId = roomId,
				FirmewareVersion = firmewareVersion,
				TestResult = testResult,
				SerialNumber = serialNumber,
				ComfortTemperature = comfortTemperature,
				EcoTemperature = ecoTemperature,
				MaxSetPointTemperature = maxSetPointTemperature,
				MinSetTemperature = minSetTemperature,
				TemperatureOffset = temperatureOffset,
				WindowOpenTemperature = windowOpenTemperature,
				WindowOpenDuration = windowOpenDuration,
				BoostDuration = boostDuration,
				BoostPercentage = boostPercentage,
				DecalcificationDay = decalcificationDay,
				DecalcificationTime = decalcificationTime,
				MaxValeSetting = maxValeSetting,
				ValveOffset = valveOffset,
				WeekTemperatureProfile = maxWeekTemperatureProfile
			};
		}

		public CMessageWallThermostat ParseWallThermostat(byte[] data, string rfAddress)
		{
			var dataLength = (int)data[0];
			var addressOfDevice = BitConverter.ToString(data, 1, 3);
			var deviceType = (MaxDeviceType)data[4];
			var roomId = (int)data[5];
			var firmewareVersion = (int)data[6];
			var testResult = (int)data[7];
			var serialNumber = BitConverter.ToString(data, 8, 10);
			var comfortTemperature = data[18] * 2.0;
			var ecoTemperature = data[19] * 2.0;
			var maxSetPointTemperature = data[20] * 2.0;
			var minSetTemperature = data[21] * 2.0;

			var weeklyProgram = data.Skip(28).ToArray();

			var maxWeekTemperatureProfile = ParseWeekProgram(weeklyProgram);

			return new CMessageWallThermostat
			{
				RfAddress = rfAddress,
				DeviceType = deviceType,
				DataLength = dataLength,
				AddressOfDevice = addressOfDevice,
				RoomId = roomId,
				FirmewareVersion = firmewareVersion,
				TestResult = testResult,
				SerialNumber = serialNumber,
				ComfortTemperature = comfortTemperature,
				EcoTemperature = ecoTemperature,
				MaxSetPointTemperature = maxSetPointTemperature,
				MinSetTemperature = minSetTemperature,
				MaxWeekTemperatureProfile = maxWeekTemperatureProfile
			};
		}

		private MaxWeekTemperatureProfile ParseWeekProgram(byte[] weeklyProgram)
		{
			var maxWeekTemperatureProfile = new MaxWeekTemperatureProfile();
			for (var day = 0; day < 7; day++)
			{
				var dayOfWeek = GetDayOfWeek(day);
				var maxDayTemperatureProfile = new MaxDayTemperatureProfile
				{
					DayOfWeek = dayOfWeek
				};

				for (var offset = 0; offset < 26; offset += 2)
				{
					var setPoint = BitConverter.ToInt16(weeklyProgram, day * 26 + offset);

					if (setPoint == 0)
						continue;

					var temperature = (setPoint >> 9) / 2.0;
					var untilTime = TimeSpan.FromMinutes((setPoint & 0x1ff) * 5.0);

					var maxTemperatureProfilSwitchPoint = new MaxTemperatureProfilSwitchPoint
					{
						Stop = GetNext(dayOfWeek) + untilTime,
						Temperature = temperature
					};
					maxDayTemperatureProfile.SwitchPoints.Add(maxTemperatureProfilSwitchPoint);
				}

				maxWeekTemperatureProfile.DayTemperatureProfiles.Add(maxDayTemperatureProfile);
			}
			return maxWeekTemperatureProfile;
		}

		private DateTime GetNext(DayOfWeek dayOfWeek)
		{
			for (var i = 0; i < 7; i++)
			{
				if (DateTime.Today.AddDays(i).DayOfWeek == dayOfWeek)
					return DateTime.Today.AddDays(i);
			}

			return DateTime.Today;
		}

		private DayOfWeek GetDayOfWeek(int dayIndex)
		{
			switch (dayIndex)
			{
				case 0:
					return DayOfWeek.Saturday;
				case 1:
					return DayOfWeek.Sunday;
				case 2:
					return DayOfWeek.Monday;
				case 3:
					return DayOfWeek.Tuesday;
				case 4:
					return DayOfWeek.Wednesday;
				case 5:
					return DayOfWeek.Thursday;
				default:
					return DayOfWeek.Friday;
			}
		}
	}
}