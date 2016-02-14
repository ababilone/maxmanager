using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MaxManager.Web.Lan.Parser.Message;
using MaxManager.Web.State;

namespace MaxManager.Web.Lan.Parser
{
	public class MMessageParser : IMessageParser
	{
		public bool Accept(string payload)
		{
			return payload.StartsWith("M:");
		}

		public IMaxMessage Parse(string payload)
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
				var id = (int)data[offset + 0];
				offset++;

				var nameLength = data[offset];
				offset++;

				var name = Encoding.UTF8.GetString(data, offset, nameLength);
				offset += nameLength;

				var groupRfAddress = BitConverter.ToString(data, offset, 3);
				offset += 3;

				var maxRoom = new MaxRoom
				{
					Id = id,
					Name = name,
					GroupRfAddress = groupRfAddress
				};

				rooms.Add(maxRoom);
			}

			int deviceCount = data[offset];
			offset++;

			var devices = new List<MaxDevice>(deviceCount);
			for (var i = 0; i < deviceCount; i++)
			{
				var maxDeviceType = (MaxDeviceType)data[offset];
				offset++;

				var radioAddress = BitConverter.ToString(data, offset, 3);
				offset += 3;
				var serialNumber = Encoding.UTF8.GetString(data, offset, 10);
				offset += 10;

				int nameLength = data[offset];
				offset++;

				var name = Encoding.UTF8.GetString(data, offset, nameLength);
				offset += nameLength;

				var roomId = (int)data[offset];
				offset++;

				var maxRoom = rooms.SingleOrDefault(room => room.Id == roomId);
				var maxDevice = new MaxDevice
				{
					SerialNumber = serialNumber,
					Name = name,
					Type = maxDeviceType,
					RfAddress = radioAddress,
					Room = maxRoom
				};

				maxRoom?.Devices.Add(maxDevice);
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
