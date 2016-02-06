using MaxManager.Web.Lan.Parser.Message;

namespace MaxManager.Web.Lan.Parser
{
	public class LEcoButtonSubMessageParser : ILSubMessageParser
	{
		public bool Accept(byte[] data)
		{
			return data.Length == 3;
		}

		public LMessage Parse(byte[] data)
		{
			return null;
		}
	}
}