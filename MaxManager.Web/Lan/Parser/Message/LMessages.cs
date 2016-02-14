using System.Collections.Generic;
using System.Linq;

namespace MaxManager.Web.Lan.Parser.Message
{
	public class LMessages : IMaxMessage
	{
		public List<LMessage> Messages { get; set; }

		public override string ToString()
		{
			return Messages.Count + " live messages:\n" + string.Join("\n", Messages.Select(m => m.ToString()));
		}
	}
}