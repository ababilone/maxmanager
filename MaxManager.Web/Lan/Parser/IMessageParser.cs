using MaxManager.Web.Lan.Parser.Message;

namespace MaxManager.Web.Lan.Parser
{
	interface IMessageParser
	{
		bool Accept(string payload);
		IMaxMessage Parse(string payload);
	}
}