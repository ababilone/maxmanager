using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaxManager.Web.Lan.Parser
{
	public class MaxParser
	{
		private readonly List<IMessageParser> _messageParsers;

		public MaxParser()
		{
			_messageParsers = new List<IMessageParser>
			{
				new HMessageParser(),
				new MMessageParser(),
				new CMessageParser()
			};
		}


		public object Parse(string payload)
		{
			var messageParser = _messageParsers.FirstOrDefault(parser => parser.Accept(payload));
			if (messageParser == null)
			{
				return null;
				//throw new Exception("Unsupported message: " + payload);
			}

			return messageParser.Parse(payload);

			/*
			//_log.Info("line:" + line);
			if (line.StartsWith("H:"))
			{
				ParseLineH(line.Substring(2));
			}
			else if (line.StartsWith("M"))
			{
				ParseLineM(line.Substring(2));
			}
			else if (line.StartsWith("C"))
			{
				ParseLineC(line.Substring(2));
			}
			else if (line.StartsWith("L"))
			{
				ParseLineL(line.Substring(2));
			}
			else {
				//_log.Warn("Unsupported message:" + line);
			}
			*/
		}

		/*

		private void ParseLineL(String line) { }

		private void ParseLineC(String line)
		{
			var tokenizer = line.Split(',');
			String rfAddress = tokenizer[0];
			String dataAsString = tokenizer[1];
			byte[] data = Convert.FromBase64String(dataAsString);

			DeviceState state = new DeviceState() { };

			int offset = 0;
			int dataLen = data[offset];
			offset++;

			UpdateProperty(state, "radioAddress", ExtractHex(data, offset, 3));
			offset += 3;

			UpdateProperty(state, "type", data[offset]);
			offset++;

			UpdateProperty(state, "?", ExtractHex(data, offset, 3));
			offset += 3;

			UpdateProperty(state, "serial", ExtractString(data, offset, 10));
			offset += 10;

			UpdateProperty(state, "rest", ExtractString(data, offset, data.Length - offset));
		}

		private void ParseLineM(String line)
		{
			var tokenizer = line.Split(',');
			String index = tokenizer[0];
			String count = tokenizer[1];
			String dataAsString = tokenizer[2];
			byte[] data = Convert.FromBase64String(dataAsString);

			var offset = 2;
			var roomCount = data[offset];
			offset++;

			var rooms = new List<MaxRoom>(roomCount);
			for (var i = 0; i < roomCount; i++)
			{
				var maxRoom = new MaxRoom();
				UpdateProperty(maxRoom, "id", data[offset + 0]);
				offset++;

				int nameLength = data[offset];
				offset++;

				UpdateProperty(maxRoom, "name", ExtractString(data, offset, nameLength));
				offset += nameLength;

				offset += 3;

				rooms.Add(maxRoom);
			}
			UpdateProperty(_state, "rooms", rooms);

			int deviceCount = data[offset];
			offset++;

			var devices = new List<MaxDevice>(deviceCount);
			for (var i = 0; i < deviceCount; i++)
			{
				var maxDevice = new MaxDevice();
				UpdateProperty(maxDevice, "deviceType", data[offset]);
				offset++;

				UpdateProperty(maxDevice, "radioAddress", ExtractHex(data, offset, 3));
				offset += 3;

				UpdateProperty(maxDevice, "serialNumber", ExtractString(data, offset, 10));
				offset += 10;

				int nameLength = data[offset];
				offset++;

				UpdateProperty(maxDevice, "name", ExtractString(data, offset, nameLength));
				offset += nameLength;

				UpdateProperty(maxDevice, "room", data[offset]);
				offset++;

				devices.Add(maxDevice);
			}
			UpdateProperty(_state, "devices", devices);

		}

		private String ExtractString(byte[] data, int start, int len)
		{
			return Encoding.UTF8.GetString(data.Skip(start - 1).Take(len).ToArray());
		}

		private String ExtractHex(byte[] data, int start, int len)
		{
			StringBuilder sb = new StringBuilder(len * 2);
			for (int i = 0; i < len; i++)
			{
				sb.Append(String.Format("%02x", data[i] & 0xff));
			}
			return sb.ToString();
		}

		private void UpdateProperty(Object obj, String property, Object value)
		{
			//_log.Info(obj.GetType().Name+ "." + property + "->" + value);
		}
		*/
	}
}