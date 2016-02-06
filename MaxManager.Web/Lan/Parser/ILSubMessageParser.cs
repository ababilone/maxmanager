using MaxManager.Web.Lan.Parser.Message;

namespace MaxManager.Web.Lan.Parser
{
	public interface ILSubMessageParser
	{
		bool Accept(byte[] data);
		LMessage Parse(byte[] data);
	}
}