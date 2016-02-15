using System.Collections.Generic;
using System.Linq;
using MaxManager.Web.Lan.Parser.Message;

namespace MaxManager.Web.Lan.Parser
{
	public class MaxParser
	{
		private readonly List<IMessageParser> _messageParsers;

		public MaxParser()
		{
			_messageParsers = new List<IMessageParser>
			{
				new AMessageParser(),
				new HMessageParser(),
				new MMessageParser(),
				new CMessageParser(),
				new LMessageParser(),
				new FMessageParser(),
				new NMessageParser()
			};
		}

		public IMaxMessage Parse(string payload)
		{
			var messageParser = _messageParsers.FirstOrDefault(parser => parser.Accept(payload));
			if (messageParser == null)
			{
				
			}
			return messageParser?.Parse(payload);
		}
	}
}