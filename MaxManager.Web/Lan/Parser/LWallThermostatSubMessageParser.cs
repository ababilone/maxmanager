using MaxManager.Web.Lan.Parser.Message;

namespace MaxManager.Web.Lan.Parser
{
	public class LWallThermostatSubMessageParser : ILSubMessageParser
	{
		public bool Accept(byte[] data)
		{
			return data.Length == 7;
		}

		public LMessage Parse(byte[] data)
		{
			return null;
		}
	}
}