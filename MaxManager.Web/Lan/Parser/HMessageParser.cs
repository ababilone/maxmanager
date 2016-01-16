namespace MaxManager.Web.Lan.Parser
{
	public class HMessageParser : IMessageParser
	{
		public bool Accept(string payload)
		{
			return payload.StartsWith("H:");
		}

		public object Parse(string payload)
		{
			var tokenizer = payload.Substring(2).Split(',');
			return new HMessage
			{
				SerialNumber = tokenizer[0],
				RfAddress = tokenizer[1],
				FirmwareVersion = tokenizer[2],
				Unknown = tokenizer[3],
				HttpConnectionId = tokenizer[4],
				DutyCycle = tokenizer[5],
				FreeMemorySlots = tokenizer[6],
				CubeDate = tokenizer[7],
				CubeTime = tokenizer[8],
				StateCubeTime = tokenizer[9],
				NtpCounter = tokenizer[10]
			};
		}
	}
}