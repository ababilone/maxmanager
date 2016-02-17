using System;
using MaxManager.Web.Lan.Commands;
using MaxManager.Web.Lan.Parser;
using MaxManager.Web.Lan.Serialization;
using MaxManager.Web.State;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace MaxManager.Web.Test.Lan.Commands
{
	[TestClass]
	public class SMaxCommandSerializationTest
	{
		[TestMethod]
		public void TestSMaxCommand()
		{
			var maxSerializer = new MaxSerializer();
			var maxRfAddress = new MaxRfAddress
			{
				Bytes = new byte[] { 0x0f, 0xda, 0xed },
				HumanReadable = "0F-DA-ED"
			};

			var roomId = 1;

			var temperatureAndModeCommand = new STemperatureAndModeMaxCommand
			{
				RfAddress = maxRfAddress,
				RoomId = roomId,
				Temperature = 38,
				Mode = MaxRoomControlMode.Manual
			};

			var bytes = maxSerializer.Serialize(temperatureAndModeCommand);
			var base64String = Convert.ToBase64String(bytes);

			Assert.AreEqual("AARAAAAAD9rtAWY=", base64String);
		}

		[TestMethod]
		public void TestSMaxCommandHolidayWithDateTime()
		{
			var maxSerializer = new MaxSerializer();
			var maxRfAddress = new MaxRfAddress
			{
				Bytes = new byte[] { 0x0f, 0xda, 0xed },
				HumanReadable = "0F-DA-ED"
			};

			var roomId = 1;

			var temperatureAndModeCommand = new STemperatureAndModeMaxCommand
			{
				RfAddress = maxRfAddress,
				RoomId = roomId,
				Temperature = 38,
				Mode = MaxRoomControlMode.Holiday,
				DateUntil = new DateTime(2011, 08, 29),
				TimeUntil = new TimeSpan(2, 0, 0)
			};

			var bytes = maxSerializer.Serialize(temperatureAndModeCommand);
			var base64String = Convert.ToBase64String(bytes);

			Assert.AreEqual("AARAAAAAD9rtAaadCwQ=", base64String);
		}
	}
}
