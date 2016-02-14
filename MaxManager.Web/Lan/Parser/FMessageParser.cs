using MaxManager.Web.Lan.Parser.Message;

namespace MaxManager.Web.Lan.Parser
{
	public class FMessageParser : IMessageParser
	{
		public bool Accept(string payload)
		{
			return payload.StartsWith("F:");
		}

		public IMaxMessage Parse(string payload)
		{
			var data = payload.Substring(2);
			var splittedData = data.Split(',');

			return new FMessage
			{
				FirstNtpServerHost = splittedData[0],
				SecondNtpServerHost = splittedData[1]
			};
		}
	}
}