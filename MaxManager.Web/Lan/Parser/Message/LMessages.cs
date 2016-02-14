using System.Collections.Generic;

namespace MaxManager.Web.Lan.Parser.Message
{
	public class LMessages : IMaxMessage
	{
		public List<LMessage> Messages { get; set; }
	}
}