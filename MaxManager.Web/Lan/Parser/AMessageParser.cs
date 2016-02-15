using MaxManager.Web.Lan.Parser.Message;

namespace MaxManager.Web.Lan.Parser
{
	public class AMessageParser : IMessageParser
	{
		public bool Accept(string payload)
		{
			return payload.StartsWith("A:");
		}

		public IMaxMessage Parse(string payload)
		{
			return new AMessage();
		}
	}
}