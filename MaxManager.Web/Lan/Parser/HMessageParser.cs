using System;
using MaxManager.Web.Lan.Parser.Message;

namespace MaxManager.Web.Lan.Parser
{
	public class HMessageParser : IMessageParser
	{
		public bool Accept(string payload)
		{
			return payload.StartsWith("H:");
		}

		public IMaxMessage Parse(string payload)
		{
			var tokenizer = payload.Substring(2).Split(',');
			var serialNumber = tokenizer[0];
			var rfAddress = tokenizer[1];
			var firmwareVersion = string.Join(".", tokenizer[2].ToCharArray());
			var unknown = tokenizer[3];
			var httpConnectionId = tokenizer[4];
			var dutyCycle = tokenizer[5];
			var freeMemorySlots = tokenizer[6];

			var cubeDate = tokenizer[7];
			var cubeTime = tokenizer[8];
			var cubeDateTime = ExtractDateTime(cubeDate, cubeTime);

			var stateCubeTime = tokenizer[9];
			var ntpCounter = Convert.ToInt32(tokenizer[10]);

			return new HMessage
			{
				SerialNumber = serialNumber,
				RfAddress = rfAddress,
				FirmwareVersion = firmwareVersion,
				Unknown = unknown,
				HttpConnectionId = httpConnectionId,
				DutyCycle = dutyCycle,
				FreeMemorySlots = freeMemorySlots,
				CubeDateTime = cubeDateTime,
				StateCubeTime = stateCubeTime,
				NtpCounter = ntpCounter
			};
		}

		private DateTime ExtractDateTime(string date, string time)
		{
			var year = ToBase10(date[0], date[1]);
			var month = ToBase10(date[2], date[3]);
			var day = ToBase10(date[4], date[5]);

			var hours = ToBase10(time[0], time[1]);
			var minutes = ToBase10(time[2], time[3]);

			return new DateTime(year, month, day, hours, minutes, 0);
		}

		private static int ToBase10(params char[] numbers)
		{
			return Convert.ToInt32(string.Join("", numbers), 16);
		}
	}
}