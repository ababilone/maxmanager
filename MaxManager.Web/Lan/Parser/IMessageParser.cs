namespace MaxManager.Web.Lan.Parser
{
	interface IMessageParser
	{
		bool Accept(string payload);
		object Parse(string payload);
	}
}