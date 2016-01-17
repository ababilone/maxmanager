using System;
using System.Collections.Generic;
using MaxControl.State;

namespace MaxManager.Web.Lan.Parser
{
	public class CMessage
	{
		public string Serial { get; set; }
		public string RadioAddress { get; set; }
		public byte Type { get; set; }
	}

	public class MMessageParser : IMessageParser
	{
		public bool Accept(string payload)
		{
			return payload.StartsWith("M:");
		}

		public object Parse(string payload)
		{
			var tokenizer = payload.Substring(2).Split(',');
			var index = tokenizer[0];
			var count = tokenizer[1];
			var dataAsString = tokenizer[2];
			var data = Convert.FromBase64String(dataAsString);

			var offset = 2;
			var roomCount = data[offset];
			offset++;

			var rooms = new List<MaxRoom>(roomCount);
			for (var i = 0; i < roomCount; i++)
			{
				var maxRoom = new MaxRoom
				{
					Id = data[offset + 0]
				};
				offset++;

				var nameLength = data[offset];
				offset++;

				maxRoom.Name = MaxUtils.ExtractString(data, offset, nameLength);
				offset += nameLength;
				offset += 3;

				rooms.Add(maxRoom);
			}

			int deviceCount = data[offset];
			offset++;

			var devices = new List<MaxDevice>(deviceCount);
			for (var i = 0; i < deviceCount; i++)
			{
				var maxDevice = new MaxDevice();
				var deviceType = data[offset];
				offset++;

				var radioAddress = MaxUtils.ExtractHex(data, offset, 3);
				offset += 3;
				maxDevice.SerialNumber = MaxUtils.ExtractString(data, offset, 10);
				offset += 10;

				int nameLength = data[offset];
				offset++;

				maxDevice.Name = MaxUtils.ExtractString(data, offset, nameLength);
				offset += nameLength;

				var room = data[offset];
				offset++;

				devices.Add(maxDevice);
			}

			return new MMessage
			{
				Rooms = rooms,
				Devices = devices
			};
		}
	}
}
