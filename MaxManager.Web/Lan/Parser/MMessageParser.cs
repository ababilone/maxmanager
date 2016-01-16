using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MaxControl.State;

namespace MaxManager.Web.Lan.Parser
{
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

				maxRoom.Name = ExtractString(data, offset, nameLength);
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

				var radioAddress = ExtractHex(data, offset, 3);
				offset += 3;
				maxDevice.SerialNumber = ExtractString(data, offset, 10);
				offset += 10;

				int nameLength = data[offset];
				offset++;

				maxDevice.Name = ExtractString(data, offset, nameLength);
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

		private string ExtractString(byte[] data, int start, int len)
		{
			return Encoding.UTF8.GetString(data.Skip(start - 1).Take(len).ToArray());
		}

		private string ExtractHex(byte[] data, int start, int len)
		{
			var sb = new StringBuilder(len * 2);
			for (var i = 0; i < len; i++)
			{
				sb.Append(string.Format("%02x", data[i] & 0xff));
			}
			return sb.ToString();
		}
	}
}
