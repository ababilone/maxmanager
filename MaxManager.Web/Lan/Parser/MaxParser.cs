using System.Collections.Generic;
using System.Linq;

namespace MaxManager.Web.Lan.Parser
{
	public class MaxParser
	{
		private readonly List<IMessageParser> _messageParsers;

		public MaxParser()
		{
			_messageParsers = new List<IMessageParser>
			{
				new HMessageParser(),
				new MMessageParser(),
				new CMessageParser(),
				new LMessageParser()
			};
		}

		public object Parse(string payload)
		{
			var messageParser = _messageParsers.FirstOrDefault(parser => parser.Accept(payload));
			if (messageParser == null)
			{
				
			}
			return messageParser?.Parse(payload);
		}
	}
}