using System;
using MaxManager.Web.Lan.Parser.Message;

namespace MaxManager.Web.Lan.Parser
{
	public class SMessageParser : IMessageParser	{
		public bool Accept(string payload)
		{
			return payload.StartsWith("S:");
		}

		public IMaxMessage Parse(string payload)
		{
			var splitted = payload.Substring(2).Split(',');

			return new SMessage
			{
				CommandProcessed = splitted[1] == "0",
				DutyCycle = Convert.ToInt32(splitted[0]),
				FreeMemorySlot = Convert.ToInt32(splitted[2])
			};
		}
	}
}