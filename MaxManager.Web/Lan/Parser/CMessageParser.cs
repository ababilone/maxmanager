using System;
using MaxControl.State;

namespace MaxManager.Web.Lan.Parser
{
	public class CMessageParser : IMessageParser
	{
		public bool Accept(string payload)
		{
			return payload.StartsWith("C:");
		}

		public object Parse(string payload)
		{
			var tokenizer = payload.Substring(2).Split(',');
			var rfAddress = tokenizer[0];
			var dataAsString = tokenizer[1];
			var data = Convert.FromBase64String(dataAsString);

			var state = new DeviceState();

			int offset = 0;
			int dataLen = data[offset];
			offset++;


			var radioAddress = MaxUtils.ExtractHex(data, offset, 3);
			offset += 3;

			var type = data[offset];
			offset++;

			var unknown = MaxUtils.ExtractHex(data, offset, 3);
			offset += 3;

			var serial = MaxUtils.ExtractString(data, offset, 10);
			offset += 10;

			var rest = MaxUtils.ExtractString(data, offset, data.Length - offset);

			return new CMessage
			{
				RadioAddress = radioAddress,
				Type = type,
				Serial = serial
			};
		}
	}
}